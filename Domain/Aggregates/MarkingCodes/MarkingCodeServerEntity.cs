
using Domain.Aggregates.Products;
using Domain.Aggregates.Sessions;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Aggregates.MarkingCodes
{
    public class MarkingCodeServerEntity(
        CodeValue code,
        ProductId productId,
        SessionId? sessionId,
        Enum_CodeType type,
        Enum_CodeStatus status,
        DateTime expiration)
    {
        public MarkingCodeId Id { get; set; }
        public CodeValue Code { get; set; } = code;
        public MarkingCodeId? ParentId { get; set; }
        public ProductId ProductId { get; set; } = productId;
        public SessionId? SessionId { get; set; } = sessionId;
        public Enum_CodeStatus Status { get; set; } = status;
        public Enum_CodeType MarkingCodeType { get; set; } = type;
        public DateTime? Expiration { get; set; } = expiration;

        #region Navigation Fields
        public ProductEntity Product { get; set; } = null!;
        public SessionEntity? Session { get; set; }
        #endregion

        public MarkingCodeServerEntity()
            : this(CodeValue.Empty, ProductId.Empty, Sessions.SessionId.Empty, Enum_CodeType.INDIVIDUAL, Enum_CodeStatus.GOTTEN, DateTime.UnixEpoch) { }
    }
}
