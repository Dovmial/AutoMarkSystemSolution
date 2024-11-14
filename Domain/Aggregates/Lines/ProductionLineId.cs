namespace Domain.Aggregates.Lines
{
    public record struct ProductionLineId(int Value)
    {
        public static  ProductionLineId Empty => new(default);
    }
}