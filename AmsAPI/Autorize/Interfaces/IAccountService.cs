using AmsAPI.Autorize.DTO;
using AmsAPI.Autorize.Models;
using SharedLibrary.OperationResult;

namespace AmsAPI.Autorize.Interfaces
{
    public interface IAccountService
    {
        Task<OperationResult> Register(UserDataRequest user);
        Task<OperationResult<string>> Login(UserDataRequest user, string ip);
        Task<OperationResult> Logout(string login);
        Task<OperationResult> SetRoles(UserSetRolesCommand userWithRoles);
        Task<OperationResult> AddRole(UserAddRoleCommand userAddRole);
        Task<OperationResult> RemoveRole(RemoveRoleFromUserCommand userRemoveRoleFromUser);
        Task<OperationResult> RemoveAccount(string login);
    }
}
