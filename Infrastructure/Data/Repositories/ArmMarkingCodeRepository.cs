using Domain.Interfaces;
using Domain.Aggregates.MarkingCodes;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;
using Domain.Aggregates.MarkingCodeHistory;

namespace Infrastructure.Data.Repositories
{
    public class ArmMarkingCodeRepository(ArmDbContext db) : BaseRepository, IMarkingCodeDAL
    {
        public async Task<OperationResult<long>> CreateAsync(MarkingCodeEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                entity.MarkingCodeHistoryEntities.Add(MarkingCodeHistory.Create());
                await db.AddAsync(entity);
                await db.SaveChangesAsync();
                return OperationResultCreator.SuccessWithValue(entity.Id.Value);
            });
        }

        public async Task<OperationResult> DeleteAsync(long id)
        {
            return await DbLogicAsync(async () =>
            {
                MarkingCodeEntity? result = await GetEntity<MarkingCodeEntity>(db, x => x.Id.Value == id);
                if (result is not null)
                {
                    db.MarkingCodes.Remove(result);
                    await db.SaveChangesAsync();
                    return OperationResultCreator.Success;
                }
                return OperationResultCreator.Failure(new NOT_FOUND());
            });
        }

        public async Task<OperationResult<IEnumerable<MarkingCodeEntity>>> GetAllAsync(Expression<Func<MarkingCodeEntity, bool>>? predicate = null)
        {
            return await DbLogicAsync(async () =>
            {
                IEnumerable<MarkingCodeEntity>? result = await GetAllByAsync(db, predicate);
                return OperationResultCreator.SuccessWithValue(result);
            });
        }

        public async Task<OperationResult<MarkingCodeEntity>> GetByAsync(Expression<Func<MarkingCodeEntity, bool>> predicate, bool isTracking = true)
        {
            return await DbLogicAsync(async () => 
            {
                MarkingCodeEntity? result = await GetEntity(db, predicate, isTracking);
                return OperationResultCreator.MayBeNotFound(result);
            });
        }

        public async Task<OperationResult> UpdateAsync(MarkingCodeEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                db.MarkingCodes.Update(entity);
                await db.SaveChangesAsync();
                return OperationResultCreator.Success;
            });
        }
    }
}
