
namespace Domain.Aggregates.Products
{
    public record struct ProductId(int Value)
    {
        public static ProductId Empty => default;
    }
}
