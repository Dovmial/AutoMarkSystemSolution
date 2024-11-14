namespace SharedLibrary.OperationResult
{

    public record class OperationResult(bool IsSuccess, Error Error);
    public record class OperationResult<TValue>(bool IsSuccess, TValue Value, Error Error);
    public class OperationResultCreator
    {
        public static OperationResult Success  => new OperationResult(true, new ERROR_EMPTY());
        public static OperationResult Failure(Error error) => new OperationResult(false, error);
        public static OperationResult<T> SuccessWithValue<T>(T value) => new OperationResult<T>(true, value, new ERROR_EMPTY());
        public static OperationResult<T> Failure<T>(Error error) => new OperationResult<T>(false, default!, error);
        public static OperationResult<T> MayBeNotFound<T>(T? result) => result switch
        {
            null => Failure<T>(new NOT_FOUND()),
            not null => SuccessWithValue(result)
        };
    }


    /*
    public record class OperationResult<T> 
    {
        private readonly bool _isSuccess;
        public readonly T? Value;
        public readonly Error? Error;
        private OperationResult(Error error)
        {
            _isSuccess = false;
            Error = error;
            Value = default;
        }

        private OperationResult(T value) 
        {
            _isSuccess = true;
            Value = value;
            Error = new ERROR_EMPTY();
        }

        public static OperationResult<T> Success(T value) => new OperationResult<T>(value);
        public static OperationResult<T> Failure(Error error) => new OperationResult<T>(error);

        public static implicit operator OperationResult<T>(T value) => new(value);
        public static implicit operator OperationResult<T>(Error error) => new(error);

        public TResult Match<TResult>(
            Func<T, TResult> success,
            Func<Error, TResult> failure) => _isSuccess 
                                                ? success(Value!) 
                                                : failure(Error!);
    }

    */
}
