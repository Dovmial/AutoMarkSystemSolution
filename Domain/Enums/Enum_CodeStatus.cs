

namespace Domain.Enums
{
    [Flags]
    public enum Enum_CodeStatus
    {
        UNKNOWN = 0,
        GOTTEN = 1,
        PRINTED= 2,
        VERIFIED = 4,
        AGGREGATED = 8,
        VERIFIED_AGGREGATED = VERIFIED | AGGREGATED,
        REJECTED = 16,
    }
}
