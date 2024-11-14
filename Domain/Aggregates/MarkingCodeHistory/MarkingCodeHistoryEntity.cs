using Domain.Aggregates.MarkingCodes;

namespace Domain.Aggregates.MarkingCodeHistory
{
    public class MarkingCodeHistoryEntity
    {
        public MarkingCodeHistoryId Id { get; set; }
        public MarkingCodeId MarkingCodeId {  get; set; }
        public DateTime TimeStamp { get; set; }
        public MarkingCodeEntity MarkingCodeEntity { get; set; } = null!;
    }
}
