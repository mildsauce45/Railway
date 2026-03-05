namespace Railway
{
    public interface IResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
    }

    public interface IValue<out T>
    {
        T Value { get; }
    }

    public interface IError<out E>
    {
        E Error { get; }
        string Message { get; }
    }

    public interface IResult<out T, out E> : IValue<T>, IError<E>
    {
    }
}
