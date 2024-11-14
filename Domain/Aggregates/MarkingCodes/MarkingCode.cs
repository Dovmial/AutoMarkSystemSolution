
using Domain.Aggregates.Products;
using Domain.Aggregates.Sessions;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Aggregates.MarkingCodes
{
    public static class MarkingCode
    {
        public static MarkingCodeServerEntity Create(
            CodeValue code,
            ProductId productId,
            SessionId sessionId,
            Enum_CodeType codeType,
            Enum_CodeStatus codeStatus,
            DateTime expiration) => 

            new MarkingCodeServerEntity(
                code,
                productId,
                sessionId,
                codeType,
                codeStatus,
                expiration);

        public static MarkingCodeServerEntity Empty => 
            new MarkingCodeServerEntity(CodeValue.Empty, ProductId.Empty, SessionId.Empty, Enum_CodeType.None, Enum_CodeStatus.UNKNOWN, DateTime.UnixEpoch);
    }
}
