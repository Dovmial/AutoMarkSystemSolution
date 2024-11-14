using AmsAPI.Autorize.DTO;
using AmsAPI.Autorize.Errors;
using AmsAPI.Autorize.Features;
using AmsAPI.Autorize.Interfaces;
using AmsAPI.Autorize.Models;
using SharedLibrary.OperationResult;

namespace AmsAPI.Autorize.Services
{
    public class AccountService(
        ILogger<AccountService> logger,
        IAccountRepository accountRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        JwtTokenGenerator tokenGenerator) : IAccountService
    {

        public async Task<OperationResult<string>> Login(UserDataRequest user, string ip)
        {
            //существует ли пользователь?
            OperationResult<Account> resultExists = await accountRepository.GetWithRoles(user.Login);
            if (!resultExists.IsSuccess)
            {
                Error error = resultExists.Error;
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure<string>(error);
            }
            Account account = resultExists.Value;

            //Верен ли пароль?
            if (!passwordHasher.Verify(user.Password, account.HashPass, account.Salt))
            {
                Error error = new ERROR_LOGIN_OR_PASSWORD_INCORRECT();
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure<string>(error);
            }

            //генерация токена

            string token = tokenGenerator.Generate(account);
            account.IpAddress = ip;
            //фиксируем вход в систему
            OperationResult resultLogin = await accountRepository.Login(account);
            if (!resultLogin.IsSuccess)
            {
                Error error = resultLogin.Error;
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure<string>(error);
            }
            logger.LogInformation("Welcome {accountLogin}", account.Login);

            return OperationResultCreator.SuccessWithValue(token);
        }

        public async Task<OperationResult> Register(UserDataRequest user)
        {
            //проверка уникальности имени пользователя при регистрации
            OperationResult<Account> resultExists = await accountRepository.Exists(user.Login);
            if (resultExists.IsSuccess)
            {
                Error error = new ERROR_USER_ALREADY_EXISTS();
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }

            (string hash, string salt) = passwordHasher.Hash(user.Password);

            Account account = new Account(user.Login, hash)
            {
                Id = new AccountId(new Guid()),
                DateOfRegistration = DateTime.UtcNow,
                Salt = salt,
            };
            //Добавление базовой роли
            string roleName = "User";
            OperationResult<Role> roleResult = await roleRepository.Exists(roleName);
            if (roleResult.IsSuccess)
            {
                if (account.Roles is null)
                    account.Roles = new List<Role>();
                account.Roles.Add(roleResult.Value);
            }
            else
            {
                logger.LogWarning((new ERROR_ROLE_UNKNOWN(roleName)).ErrorCode);
            }
            //сохранение аккаунта
            await accountRepository.Add(account);

            logger.LogInformation("Welcome new user: {userLogin}", user.Login);
            return OperationResultCreator.Success;
        }

        public async Task<OperationResult> SetRoles(UserSetRolesCommand userWithRoles)
        {
            //поиск аккаунта
            OperationResult<Account> resultExists = await accountRepository.Exists(userWithRoles.Login);
            if (!resultExists.IsSuccess)
            {
                Error error = new NOT_FOUND();
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }

            //проверка предлагаемых ролей
            OperationResult<Role> resultRole;
            List<Role> roles = new List<Role>();
            foreach (string role in userWithRoles.roles)
            {
                resultRole = await roleRepository.Exists(role);
                if (!resultRole.IsSuccess)
                {
                    Error error = new ERROR_ROLE_UNKNOWN(role);
                    logger.LogWarning(error.ErrorCode);
                    return OperationResultCreator.Failure(error);
                }
                roles.Add(resultRole.Value);
            }

            //изменение списка ролей у аккаунта
            Account account = resultExists.Value;
            if(account.Roles is not null)
                account.Roles.Clear();
            account.Roles = roles;
            return await accountRepository.Update(account);
        }

        public async Task<OperationResult> AddRole(UserAddRoleCommand userWithRole)
        {
            //поиск аккаунта
            OperationResult<Account> accountResult = await accountRepository.GetWithRoles(userWithRole.Login);
            if (!accountResult.IsSuccess)
            {
                Error error = accountResult.Error;
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }

            //поиск роли
            OperationResult<Role> resultRole = await roleRepository.Exists(userWithRole.Role);
            if (!resultRole.IsSuccess)
            {
                Error error = resultRole.Error;
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }

            Account entity = accountResult.Value;
            entity.Roles.Add(resultRole.Value);
            return await accountRepository.Update(entity);
        }

        public async Task<OperationResult> RemoveRole(RemoveRoleFromUserCommand userRole)
        {
            //поиск аккаунта
            OperationResult<Account> accountResult = await accountRepository.GetWithRoles(userRole.Login);
            if (!accountResult.IsSuccess)
            {
                Error error = accountResult.Error;
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }

            //поиск роли у аккаунта
            Account entity = accountResult.Value;
            Role? roleEntity = entity.Roles.FirstOrDefault(x => x.Name == userRole.Role);
            if(roleEntity is null)
            {
                Error error = new NOT_FOUND(userRole.Role);
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }

            //удаление роли
            entity.Roles.Remove(roleEntity);
            return await accountRepository.Update(entity);
        }

        public async Task<OperationResult> RemoveAccount(string login)
        {
            OperationResult<Account> accountResult = await accountRepository.GetWithRoles(login);
            if (!accountResult.IsSuccess)
            {
                Error error = accountResult.Error;
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }
           return  await accountRepository.Delete(accountResult.Value);
        }

        public async Task<OperationResult> Logout(string login)
        {
            OperationResult<Account> accountResult = await accountRepository.Exists(login);
            if (!accountResult.IsSuccess)
            {
                Error error = accountResult.Error;
                logger.LogInformation(error.ErrorCode);
                return OperationResultCreator.Failure(error);
            }
            Account account = accountResult.Value;
            return await accountRepository.LogOut(account);
        }
    }
}
