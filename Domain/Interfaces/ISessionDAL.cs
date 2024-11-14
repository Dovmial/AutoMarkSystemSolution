using Domain.Aggregates.Sessions;

namespace Domain.Interfaces
{
    public interface ISessionDAL: IGenericEntityDAL<SessionEntity, Guid>
    {
    }
}
