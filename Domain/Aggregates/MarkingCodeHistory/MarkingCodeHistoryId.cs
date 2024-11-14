
namespace Domain.Aggregates.MarkingCodeHistory
{
    public record struct MarkingCodeHistoryId(int Value)
    {
        public static MarkingCodeHistoryId Empty => new MarkingCodeHistoryId(default);
    }
}
