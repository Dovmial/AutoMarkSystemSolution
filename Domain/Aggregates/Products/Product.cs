
using Domain.ValueObjects;

namespace Domain.Aggregates.Products
{
    public static class Product
    {
        public static ProductEntity Create(string name, GTIN gtin, int serialLength, GTIN? gtinGroup = null) => new ProductEntity(name, gtin, serialLength, gtinGroup);
        public static ProductEntity Empty => new ProductEntity(string.Empty, GTIN.Empty, default);
    }
}
