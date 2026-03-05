namespace Railway
{
    public static class Combine
    {
        private const string _defaultErrorMessageSeparator = ", ";

        public static Result Results(IEnumerable<Result> results)
        {
            var failedResults = results.Where(r => r.IsFailure).ToArray();

            return failedResults.Length == 0
                ? Result.Success()
                : Result.Failure();
        }

        public static async Task<Result> ResultsAsync(IEnumerable<Task<Result>> resultTasks, bool configureAwait = false)
        {
            var results = await Task.WhenAll(resultTasks).ConfigureAwait(configureAwait);

            return Results(results);
        }

        public static Task<Result> ResultsAsync(IEnumerable<Func<Task<Result>>> taskFactories, bool configureAwait = false) =>
            ResultsAsync(taskFactories.Select(f => f.Invoke()), configureAwait);

        public static Result Results<T>(IEnumerable<Result<T>> results)
        {
            var untyped = results.Select(r => r.IsSuccess ? Result.Success() : Result.Failure());

            return Results(untyped);
        }

        public static async Task<Result> ResultsAsync<T>(IEnumerable<Task<Result<T>>> resultTasks, bool configureAwait = false)
        {
            var results = await Task.WhenAll(resultTasks).ConfigureAwait(configureAwait);

            return Results(results);
        }

        public static Task<Result> ResultsAsync<T>(IEnumerable<Func<Task<Result<T>>>> taskFactories, bool configureAwait = false) =>
            ResultsAsync(taskFactories.Select(f => f.Invoke()), configureAwait);

        public static Result<bool, TError> Results<T, TError>(IEnumerable<Result<T, TError>> results, TError errorValue, string? errorMessageSeparator = null) =>
            Results(results, _ => Result.Success<bool, TError>(true), errorValue, errorMessageSeparator);

        public static Result<TValue, TError> Results<TValue, TError>(IEnumerable<Result<TValue, TError>> results, TValue successValue, TError errorValue, string? errorMessageSeparator = null) =>
            Results(results, _ => Result.Success<TValue, TError>(successValue), errorValue, errorMessageSeparator);

        public static Result<TResult, TError> Results<T, TResult, TError>(
            IEnumerable<Result<T, TError>> results,
            Func<IEnumerable<Result<T, TError>>, Result<TResult, TError>> successFactory,
            TError errorValue,
            string? errorMessageSeparator = null)
        {
            var failedResults = results
                .Where(r => r.IsFailure)
                .OfType<IError<TError>>()
                .ToArray();

            if (failedResults.Length == 0)
                return successFactory(results);

            return Result.Failure<TResult, TError>(errorValue, AggregateErrors(failedResults, errorMessageSeparator));
        }

        private static string AggregateErrors<TError>(IEnumerable<IError<TError>> errors, string? errorMessageSeparator = null) =>
            string.Join(errorMessageSeparator ?? _defaultErrorMessageSeparator, errors.Select(e => e.Message));
    }
}
