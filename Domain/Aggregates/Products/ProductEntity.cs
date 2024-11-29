
using Domain.Aggregates.Lines;
using Domain.Aggregates.Sessions;
using Domain.ValueObjects;

namespace Domain.Aggregates.Products
{
    public class ProductEntity(string name, GTIN gtin, int serialLength, GTIN? gtinGroup = null)
    {
        public ProductId Id { get; set; }
        public string Name { get; set; } = name;
        public int SerialLength { get; set; } = serialLength;
        public GTIN Gtin { get; set; } = gtin;
        public GTIN? GtinGroup { get; set; } = gtinGroup;

        public ICollection<SessionEntity> Sessions { get; set; } = [];
        public ICollection<ProductionLineEntity> ProductionLines { get; set; } = [];
        public ProductEntity() : this(string.Empty, GTIN.Empty, default) { }

    }
}
