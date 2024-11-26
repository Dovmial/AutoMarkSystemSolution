using Domain.Enums;

namespace Application.PostOffice
{
    public class SessionStateEventArgs(Enum_SessionState sessionState) : EventArgs 
    {
        public Enum_SessionState SessionState { get; init; } = sessionState;
    }
}