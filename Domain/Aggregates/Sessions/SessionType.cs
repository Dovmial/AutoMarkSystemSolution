

using Domain.Enums;

namespace Domain.Aggregates.Sessions
{
    public abstract record SessionType(Enum_SessionType SesionType);
    public record UnknownType(): SessionType(Enum_SessionType.UNKNOWN);
    public record Serialization(): SessionType(Enum_SessionType.SERIALIZATION);
    public record AggregationPack(): SessionType(Enum_SessionType.AGGREGATION_PACK);
    public record AggregationPallet(): SessionType(Enum_SessionType.AGGREGATION_PALLET);
}
