

using Domain.Enums;

namespace Application.DTOs
{
    public class SessionOptionsEventArgs(
        string productName,
        string productionDate,
        string lineName,
        Enum_SessionState sessionState,
        Enum_SessionType sessionType) : EventArgs
    {
        public string ProductName { get; init; } = productName;
        public string LineName { get; init; } = lineName;
        public string ProductionDate { get; init; } = productionDate;
        public Enum_SessionState SessionState { get; init; } = sessionState;
        public Enum_SessionType SessionType { get; init; } = sessionType;
    }
}
