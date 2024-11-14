

using Domain.Aggregates.Products;
using Domain.Aggregates.Sessions;

namespace Domain.Aggregates.Lines
{
    public sealed class ProductionLineEntity
    {
        public ProductionLineId Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<SessionEntity> Sessions { get; set; } = [];
        public ICollection<ProductEntity> Products { get; set; } = [];
    }
}
