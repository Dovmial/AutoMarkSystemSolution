using AmsAPI.Autorize.Data;
using AmsAPI.Autorize.Interfaces;
using AmsAPI.Autorize.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;

namespace AmsAPI.Autorize.Repositories
{
    public class AccountRepository(UserDbContext db) : IAccountRepository
    {
        public async Task<OperationResult<Guid>> Add(Account account)
        {
            try
            {
                account.Id = new AccountId(Guid.NewGuid());
                await db.Accounts.AddAsync(account);
                await db.SaveChangesAsync();
                return OperationResultCreator.SuccessWithValue(account.Id.Value);
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure<Guid>(new ERROR_FROM_EXCEPTION(ex));
            }
        }
        public async Task<OperationResult> Login(Account user)
        {
            user.LastLogin = DateTime.UtcNow;
            user.IsOnline = true;
            return await Update(user);
                
        }

        public async Task<OperationResult> LogOut(Account account)
        {
            account.IsOnline = false;
            account.IpAddress = string.Empty;
            account.LastLogout = DateTime.UtcNow;
            return await Update(account);
        }

        public Task<OperationResult<Account>> Exists(string login)
        {
            Account? account = db.Accounts.FirstOrDefault(x => x.Login == login);
            OperationResult<Account> result = account is not null
                ? OperationResultCreator.SuccessWithValue(account!)
                : OperationResultCreator.Failure<Account>(new NOT_FOUND());
            return Task.FromResult(result);
        }

        public Account GetByUserName(string userName)
        {
            Account? account = db.Accounts.FirstOrDefault(x => x.Login == userName);
            return account is not null
                ? account
                : Account.Empty;
        }

        public async Task<OperationResult<ICollection<Account>>> GetAll(Expression<Func<Account, bool>>? filter = null)
        {
            IQueryable<Account> query = db.Accounts.AsQueryable().AsNoTracking();
            if(filter is not null)
                query = query.Where(filter);
            ICollection<Account> result = await query.ToListAsync();
            return OperationResultCreator.SuccessWithValue(result);
        }

        public async Task<OperationResult> Update(Account account)
        {
            try
            {
                db.Accounts.Update(account);
                await db.SaveChangesAsync();
                return OperationResultCreator.Success;
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        public async Task<OperationResult> Delete(Account accountWithRoles)
        {
            try
            {
                db.Accounts.Remove(accountWithRoles);
                await db.SaveChangesAsync();
                return OperationResultCreator.Success;
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        public async Task<OperationResult<Account>> GetWithRoles(string user)
        {
            try
            {
                Account? account = await db.Accounts
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Login == user);
                return account is not null
                    ? OperationResultCreator.SuccessWithValue(account)
                    : OperationResultCreator.Failure<Account>(new NOT_FOUND(user));
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure<Account>(new ERROR_FROM_EXCEPTION(ex));
            }
        }
    }
}
