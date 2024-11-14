using SharedLibrary.OperationResult;
using System.Linq.Expressions;


namespace Domain.Interfaces
{
    public interface IGenericEntityDAL<TEntity, TId> where TEntity : class where TId : struct
    {
        Task<OperationResult<TId>> CreateAsync(TEntity entity);
        Task<OperationResult> UpdateAsync(TEntity entity);
        Task<OperationResult> DeleteAsync(TId id);
        Task<OperationResult<IEnumerable<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<OperationResult<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking);
    }
}
