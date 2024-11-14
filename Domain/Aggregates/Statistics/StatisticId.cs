
namespace Domain.Aggregates.Statistics
{
    public record struct StatisticId(int Value)
    {
        public StatisticId Empty => new StatisticId(default);
    }
}
