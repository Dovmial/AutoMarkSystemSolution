

using Domain.Aggregates.MarkingCodes;

namespace Domain.Aggregates.MarkingCodeHistory
{
    public static class MarkingCodeHistory
    {
        public static MarkingCodeHistoryEntity Create() => new MarkingCodeHistoryEntity()
        {
            Id = MarkingCodeHistoryId.Empty,
            TimeStamp = DateTime.UtcNow,
        };
    }
}
