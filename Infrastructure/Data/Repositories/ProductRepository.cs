using Domain.Interfaces;
using Domain.Aggregates.Products;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;
using Domain.Aggregates.Lines;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository(ArmDbContext db) : BaseRepository, IProductDAL
    {
        public async Task<OperationResult<int>> CreateAsync(ProductEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                await db.Products.AddAsync(entity);
                await db.SaveChangesAsync();
                return OperationResultCreator.SuccessWithValue(entity.Id.Value);
            });
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            return await DbLogicAsync(async () =>
            {
                var result = await GetEntity<ProductEntity>(db, x => x.Id.Value == id);
                if (result is not null)
                {
                    db.Products.Remove(result);
                    await db.SaveChangesAsync();
                    return OperationResultCreator.Success;
                }
                return OperationResultCreator.Failure(new NOT_FOUND());
            });
        }

        public async Task<OperationResult<IEnumerable<ProductEntity>>> GetAllAsync(Expression<Func<ProductEntity, bool>>? predicate = null)
        {
            return await DbLogicAsync(async () =>
            {
                IEnumerable<ProductEntity>? result = await GetAllByAsync(db, predicate);
                return OperationResultCreator.MayBeNotFound(result);
            });
        }

        public async Task<OperationResult<ProductEntity>> GetByAsync(Expression<Func<ProductEntity, bool>> predicate, bool isTracking = true)
        {
            return await DbLogicAsync(async () =>
            {
                ProductEntity? product = await GetEntity(db, predicate, isTracking);
                return OperationResultCreator.MayBeNotFound(product);
            });
        }

        public async Task<OperationResult> UpdateAsync(ProductEntity entity)
        {
            return await DbLogicAsync(async () =>
            {
                db.Products.Update(entity);
                await db.SaveChangesAsync();
                return OperationResultCreator.Success;
            });
        }
    }
}
