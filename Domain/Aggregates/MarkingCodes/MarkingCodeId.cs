

namespace Domain.Aggregates.MarkingCodes
{
    public record struct MarkingCodeId(long Value)
    {
        public static MarkingCodeId Empty = new MarkingCodeId(default);
    }
}
