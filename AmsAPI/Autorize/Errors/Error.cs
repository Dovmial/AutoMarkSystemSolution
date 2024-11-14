using SharedLibrary.OperationResult;

namespace AmsAPI.Autorize.Errors
{
    public sealed record class ERROR_USER_ALREADY_EXISTS() : Error("User already exists!");
    public sealed record class ERROR_LOGIN_OR_PASSWORD_INCORRECT() : Error("Login or password is incorrected");
    public sealed record class ERROR_ROLE_UNKNOWN(string role) : Error($"Role '{role}' does not exist");
}
