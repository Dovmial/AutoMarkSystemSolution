

using SharedLibrary.OperationResult;

namespace Application.Errors
{
    public sealed record class ERROR_NULL_SESSION() : Error("Session is not created!");
    
    
}
