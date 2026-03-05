namespace Railway
{
    public static class CreateResult
    {
        public static Result<T, E> Success<T, E>(T result) =>
            Result.Success<T, E>(result);

        public static Result<string, E> Success<E>() =>
            Result.Success<string, E>(string.Empty);

        public static Result<T, E> Failure<T, E>(E error, string? message = null) =>
            Result.Failure<T, E>(error, message ?? string.Empty);

        public static Result<string, E> Failure<E>(E error, string? message = null) =>
            Result.Failure<string, E>(error, message ?? string.Empty);
    }
}
