

using Domain.Aggregates.MarkingCodeHistory;
using Domain.Aggregates.Sessions;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Aggregates.MarkingCodes
{
    public class MarkingCodeEntity
    {
        public MarkingCodeId Id { get; set; }
        public CodeValue Code { get; set; } = null!;
        public SessionId? SessionId { get; set; }
        public Enum_CodeType CodeType { get; set; }
        public MarkingCodeId? ParentId { get; set; }
        public SessionEntity? Session { get; set; }
        public ICollection<MarkingCodeHistoryEntity> MarkingCodeHistoryEntities { get; set; } = [];
    }
}
