using AmsAPI.Autorize.Data;
using AmsAPI.Autorize.Errors;
using AmsAPI.Autorize.Interfaces;
using AmsAPI.Autorize.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;

namespace AmsAPI.Autorize.Repositories
{
    public class RoleRepository(UserDbContext db) : IRoleRepository
    {
        public async Task<OperationResult<int>> Add(Role account)
        {
            try
            {
                await db.Roles.AddAsync(account);
                await db.SaveChangesAsync();
                return OperationResultCreator.SuccessWithValue(account.Id.Value);
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure<int>(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        public async Task<OperationResult> Delete(Role account)
        {
            try
            {
                db.Roles.Remove(account);
                await db.SaveChangesAsync();
                return OperationResultCreator.Success;
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        public async Task<OperationResult<Role>> Exists(string role)
        {
            try
            {
                Role? roleEntity = await db.Roles.FirstOrDefaultAsync(x => x.Name == role);
                return roleEntity is not null
                    ? OperationResultCreator.SuccessWithValue(roleEntity)
                    : OperationResultCreator.Failure<Role>(new ERROR_ROLE_UNKNOWN(role));
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure<Role>(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        public async Task<OperationResult<ICollection<Role>>> GetAll(Expression<Func<Role, bool>>? predicate)
        {
            try
            {
                IQueryable<Role> roles = db.Roles.AsQueryable().AsNoTracking();
                if(predicate is not null)
                    roles = roles.Where(predicate);
                ICollection<Role> result = await roles.ToListAsync();
                return OperationResultCreator.SuccessWithValue(result);
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure<ICollection<Role>>(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        public async Task<OperationResult> Update(Role account)
        {
            try
            {
                db.Update(account);
                await db.SaveChangesAsync();
                return OperationResultCreator.Success;
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure(new ERROR_FROM_EXCEPTION(ex));
            }
        }
    }
}
