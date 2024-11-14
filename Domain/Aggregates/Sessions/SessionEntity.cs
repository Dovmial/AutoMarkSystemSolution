

using Domain.Aggregates.Lines;
using Domain.Aggregates.MarkingCodes;
using Domain.Aggregates.Products;
using Domain.Enums;

namespace Domain.Aggregates.Sessions
{
    public  class SessionEntity
    {
        public SessionId Id { get; set; } = new SessionId(Guid.NewGuid());
        public Enum_SessionType SessionType { get; set; } = Enum_SessionType.UNKNOWN;
        public ProductId ProductId { get; set; }
        public ProductionLineId ProductionLineId { get; set; }
        public Enum_SessionState State { get; set; } = Enum_SessionState.UNKNOWN;
        public DateTime ProductionDate { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Closed { get; set; }

        //public IEnumerable<StatisticEntity> Statistics { get; set; } = null!;
        public ProductEntity Product { get; set; } = null!;
        public ProductionLineEntity ProductionLine { get; set; } = null!;
        public ICollection<MarkingCodeEntity> MarkingCodes { get; set; } = [];
    }
}
