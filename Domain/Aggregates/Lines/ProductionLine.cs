
namespace Domain.Aggregates.Lines
{
    public static class ProductionLine
    {
        public static ProductionLineEntity Create(bool isActive, string name, string? description = null)
            => new ProductionLineEntity()
            {
                Name = name,
                Description = description,
                IsActive = isActive
            };
    }
}
