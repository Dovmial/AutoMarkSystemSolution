
using Domain.Aggregates.MarkingCodes;
using Domain.Aggregates.Sessions;

namespace Domain.Aggregates.Statistics
{
    public class StatisticEntity
    {
        public StatisticId StatisticId { get; set; }
        public MarkingCodeType MarkingCodeType { get; set; }
        public int VerifiedCounter { get; set; }
        public int AggregatedCounter { get; set; }
        public SessionId SessionId { get; set; }

        public SessionEntity Session { get; set; }
    }
}
