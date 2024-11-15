namespace SharedLibrary.OperationResult
{
    public abstract record class Error(string ErrorCode);
    public sealed record class ERROR_EMPTY(): Error("");
    public sealed record class NOT_FOUND(string name = "") : Error($"NOT_FOUND {name}");
    public sealed record class NOREAD() : Error("NOREAD");
    public sealed record class INVALID_DATA(string? description = null) : Error($"INVALID_DATA: {description}");
    public sealed record class NOT_VALID_GTIN_ERROR(): Error("GTIN_NOT_VALID");
    public sealed record class SERIAL_NUMBER_LENGTH_ERROR(): Error("SERIAL_NUMBER_INCORRECT_LENGTH");
    public sealed record class CRYPTOKEY_ERROR(): Error("CRYPTO_KEY_ERROR");
    public sealed record class ERROR_FROM_EXCEPTION(Exception ex): Error(ex.MessageWithInners());
}