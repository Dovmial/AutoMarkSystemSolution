using Domain.Aggregates.Sessions;
using SharedLibrary.OperationResult;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface ISessionDAL: IGenericEntityDAL<SessionEntity, Guid>
    {
        Task<OperationResult<SessionEntity>> GetByFullAsync(Expression<Func<SessionEntity, bool>> predicate);
    }
}
