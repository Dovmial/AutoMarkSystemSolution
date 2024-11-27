using Domain.Aggregates.Products;
using Domain.Aggregates.Sessions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class SessionRepository(ArmDbContext dbContext) : BaseRepository, ISessionDAL
    {
        public async Task<OperationResult<Guid>> CreateAsync(SessionEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                dbContext.Sessions.Add(entity);
                await dbContext.SaveChangesAsync();
                return OperationResultCreator.SuccessWithValue(entity.Id.Value);
            });
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            return await DbLogicAsync(async () =>
            {
                SessionEntity? result = await GetEntity<SessionEntity>(dbContext, x => x.Id.Value == id);
                if (result is not null)
                {
                    dbContext.Sessions.Remove(result);
                    await dbContext.SaveChangesAsync();
                    return OperationResultCreator.Success;
                }
                return OperationResultCreator.Failure(new NOT_FOUND());
            });
        }

        public async Task<OperationResult<IEnumerable<SessionEntity>>> GetAllAsync(Expression<Func<SessionEntity, bool>>? predicate = null)
        {

            var result = await DbLogicAsync(async () =>
            {
                IEnumerable<SessionEntity>? sessions = await GetAllByAsync(dbContext, predicate);
                return OperationResultCreator.MayBeNotFound(sessions);
            });
            return result;
        }

        public async Task<OperationResult<SessionEntity>> GetByAsync(Expression<Func<SessionEntity, bool>> predicate, bool isTracking)
        {
            return await DbLogicAsync(async () =>
            {
                SessionEntity? entity = await GetEntity(dbContext, predicate, isTracking);
                return OperationResultCreator.MayBeNotFound(entity);
            });
        }

        public async Task<OperationResult<SessionEntity>> GetByFullAsync(Expression<Func<SessionEntity, bool>> predicate)
        {
            return await DbLogicAsync(async () =>
            {
                SessionEntity? entity = await dbContext.Sessions
                    .Include(s => s.Product)
                    .Include(s => s.ProductionLine)
                    .FirstOrDefaultAsync(predicate);
                return OperationResultCreator.MayBeNotFound(entity);
            });
        }

        public async Task<OperationResult> UpdateAsync(SessionEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                dbContext.Sessions.Update(entity);
                await dbContext.SaveChangesAsync();
                return OperationResultCreator.Success;
            });
        }
    }
}
