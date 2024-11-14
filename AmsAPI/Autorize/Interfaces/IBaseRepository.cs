
using SharedLibrary.OperationResult;
using System.Linq.Expressions;

namespace AmsAPI.Autorize.Interfaces
{
    public interface IBaseRepository<TEntity, TId> 
        where TEntity : class
        where TId: struct
    {
        Task<OperationResult<TId>> Add(TEntity entity);
        Task<OperationResult<TEntity>> Exists(string name);
        Task<OperationResult<ICollection<TEntity>>> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
        Task<OperationResult> Update(TEntity entity);
        Task<OperationResult> Delete(TEntity entity);
    }
}
