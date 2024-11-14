
namespace Domain.Aggregates.Sessions
{
    public record struct SessionId(Guid Value)
    {
        public static SessionId Empty => new SessionId(Guid.Empty);
    }
}
