

using Domain.Enums;

namespace Domain.Aggregates.MarkingCodes
{
    public abstract record class MarkingCodeType(Enum_CodeType CodeType);
    public record IndividualCode()  :MarkingCodeType(Enum_CodeType.INDIVIDUAL);
    public record GroupCode()       :MarkingCodeType(Enum_CodeType.GROUP);
    public record SSCC_Code()       :MarkingCodeType(Enum_CodeType.SSCC);
    public record Empty_Code()      :MarkingCodeType(Enum_CodeType.None);
}
