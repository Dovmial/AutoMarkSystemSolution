

using Domain.Aggregates.MarkingCodes;

namespace Domain.Aggregates.MarkingCodeHistory
{
    public static class MarkingCodeHistoryEntityExt
    {
        public static MarkingCodeHistoryEntity Create() => new MarkingCodeHistoryEntity()
        {
            Id = MarkingCodeHistoryId.Empty,
            TimeStamp = DateTime.UtcNow,
        };
    }
}
