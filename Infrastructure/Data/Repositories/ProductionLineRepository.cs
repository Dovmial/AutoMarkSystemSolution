
using Domain.Aggregates.Lines;
using Domain.Interfaces;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class ProductionLineRepository(ArmDbContext db) : BaseRepository, IProductionLineDAL
    {
        public async Task<OperationResult<int>> CreateAsync(ProductionLineEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                db.ProductionLines.Add(entity);
                await db.SaveChangesAsync();
                return OperationResultCreator.SuccessWithValue(entity.Id.Value);
            });
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            return await DbLogicAsync(async () =>
            {
                ProductionLineEntity? line = await GetEntity<ProductionLineEntity>(db, x => x.Id.Value == id, isTracking: true);
                if (line is not null) {
                    db.ProductionLines.Remove(line);
                    await db.SaveChangesAsync();
                    return OperationResultCreator.Success;
                }
                return OperationResultCreator.Failure(new NOT_FOUND());
            });
        }

        public async Task<OperationResult<IEnumerable<ProductionLineEntity>>> GetAllAsync(Expression<Func<ProductionLineEntity, bool>>? predicate = null)
        {
            return await DbLogicAsync(async () =>
            {
                var result = await GetAllByAsync(db, predicate);
                return OperationResultCreator.MayBeNotFound(result);
            });
          
        }

        public async Task<OperationResult<ProductionLineEntity>> GetByAsync(Expression<Func<ProductionLineEntity, bool>> predicate, bool isTracking = true)
        {
            return await DbLogicAsync(async () =>
            {
                ProductionLineEntity? line = await GetEntity(db, predicate, isTracking);
                return OperationResultCreator.MayBeNotFound(line);
            });
        }

        public async Task<OperationResult> UpdateAsync(ProductionLineEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                db.ProductionLines.Update(entity);
                await db.SaveChangesAsync();
                return OperationResultCreator.Success;
            });
        }
    }
}
