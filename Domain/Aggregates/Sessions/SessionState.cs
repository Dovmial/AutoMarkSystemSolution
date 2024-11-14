

using Domain.Enums;

namespace Domain.Aggregates.Sessions
{
    public abstract record SessionState(Enum_SessionState sessionState);
    public sealed record UNKNOWN_STATE(): SessionState(Enum_SessionState.UNKNOWN);
    public sealed record STARTED()   : SessionState(Enum_SessionState.STARTED);
    public sealed record STOPPED()   : SessionState(Enum_SessionState.STOPPED);
    public sealed record CLOSED()    : SessionState(Enum_SessionState.CLOSED);
    public sealed record SENT()      : SessionState(Enum_SessionState.SENT);
    public sealed record PROCESSED() : SessionState(Enum_SessionState.PROCESSED);

}
