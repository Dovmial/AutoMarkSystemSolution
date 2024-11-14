using Microsoft.EntityFrameworkCore;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public abstract class BaseRepository
    {

        protected async Task<TEntity?> GetEntity<TEntity>(ArmDbContext db, Expression<Func<TEntity, bool>> predicate, bool isTracking = true) where TEntity : class
        {
            IQueryable<TEntity> query = isTracking ? db.Set<TEntity>() : db.Set<TEntity>().AsNoTracking();
            TEntity? line = await query.FirstOrDefaultAsync(predicate);
            return line;
        }

        protected async Task<IEnumerable<TEntity>> GetAllByAsync<TEntity>(ArmDbContext db, Expression<Func<TEntity, bool>>? predicate) where TEntity : class
        {
            IQueryable<TEntity> query = db.Set<TEntity>().AsQueryable().AsNoTracking();
            if (predicate is not null)
                query = query.Where(predicate);
            return await query.ToListAsync();
        }

        protected OperationResult<TValue> DbLogic<TValue>(Func<OperationResult<TValue>> action)
        {
            try
            {
                OperationResult<TValue> result = action();
                return result;
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure<TValue>(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        protected async Task<OperationResult<TValue>> DbLogicAsync<TValue>(Func<Task<OperationResult<TValue>>> action)
        {
            try
            {
                OperationResult<TValue> result = await action();
                return result;
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure<TValue>(new ERROR_FROM_EXCEPTION(ex));
            }
        }

        protected async Task<OperationResult> DbLogicAsync(Func<Task<OperationResult>> action)
        {
            try
            {
                OperationResult result = await action();
                return result;
            }
            catch (Exception ex)
            {
                return OperationResultCreator.Failure(new ERROR_FROM_EXCEPTION(ex));
            }
        }
    }
}
