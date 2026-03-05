namespace Railway
{
    public static partial class ResultExtensions
    {
        public static async Task<Result<TOut>> OnSuccessAsync<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, Task<Result<TOut>>> newFuncResult)
        {
            if (result.IsFailure)
                return Result.Failure<TOut>();

            return await newFuncResult(result.Value).ConfigureAwait(false);
        }

        public static async Task<Result<TOut>> OnSuccessAsync<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, Task<Result<TOut>>> newFuncResult,
            Action<TOut> action,
            bool continueOnCapturedContext = false)
        {
            if (result.IsFailure)
                return Result.Failure<TOut>();

            var funcResult = await newFuncResult(result.Value).ConfigureAwait(continueOnCapturedContext);
            if (funcResult.IsFailure)
                return funcResult;

            action(funcResult.Value);

            return funcResult;
        }

        public static async Task<Result<TOut, E>> OnSuccessAsync<TIn, TOut, E>(
            this Result<TIn> result,
            Func<TIn, Task<Result<TOut, E>>> newFuncResult,
            Action<TOut> action,
            bool continueOnCapturedContext = false)
        {
            if (result.IsFailure)
                return Result.Failure<TOut, E>(default!, $"Converted failed Result<{typeof(TIn).Name}> to Result<{typeof(TOut).Name},{typeof(E).Name}>");

            var funcResult = await newFuncResult(result.Value).ConfigureAwait(continueOnCapturedContext);
            if (funcResult.IsFailure)
                return funcResult;

            action(funcResult.Value);

            return funcResult;
        }

        public static async Task<Result<T, E>> OnSuccessAsync<T, E>(
            this Task<Result<T, E>> result,
            Action<T> action,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);
            if (awaitedResult.IsFailure)
                return awaitedResult;

            action(awaitedResult.Value);

            return awaitedResult;
        }

        public static async Task<Result<TOut, E>> OnSuccessAsync<TIn, TOut, E>(
            this Task<Result<TIn, E>> result,
            Func<Result<TOut, E>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);
            if (awaitedResult.IsFailure)
                return Result.Failure<TOut, E>(default!, awaitedResult.Message);

            return newFuncResult();
        }

        public static async Task<Result<TOut, E>> OnSuccessAsync<TIn, TOut, E>(
            this Task<Result<TIn, E>> result,
            Func<TIn, Result<TOut, E>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);
            if (awaitedResult.IsFailure)
                return Result.Failure<TOut, E>(default!, awaitedResult.Message);

            return newFuncResult(awaitedResult.Value);
        }

        public static async Task<Result<TOut, E>> OnSuccessAsync<TIn, TOut, E>(
            this Task<Result<TIn, E>> result,
            Func<Task<Result<TOut, E>>> newFuncResult,
            Action<TOut> action,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);
            if (awaitedResult.IsFailure)
                return Result.Failure<TOut, E>(awaitedResult.Error, awaitedResult.Message);

            var funcResult = await newFuncResult().ConfigureAwait(continueOnCapturedContext);
            if (funcResult.IsFailure)
                return Result.Failure<TOut, E>(funcResult.Error, funcResult.Message);

            action(funcResult.Value);

            return funcResult;
        }

        public static async Task<Result<TOut, E>> OnSuccessAsync<TIn, TOut, E>(
            this Task<Result<TIn, E>> result,
            Func<TIn, Task<Result<TOut, E>>> newFuncResult,
            Action<TOut> action,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);
            if (awaitedResult.IsFailure)
                return Result.Failure<TOut, E>(awaitedResult.Error, awaitedResult.Message);

            var funcResult = await newFuncResult(awaitedResult.Value).ConfigureAwait(continueOnCapturedContext);
            if (funcResult.IsFailure)
                return Result.Failure<TOut, E>(funcResult.Error, funcResult.Message);

            action(funcResult.Value);

            return funcResult;
        }

        public static async Task<Result<TOut, E>> OnSuccessAsync<TIn, TOut, E>(
            this Task<Result<TIn, E>> result,
            Func<Task<Result<TOut, E>>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);
            if (awaitedResult.IsFailure)
                return Result.Failure<TOut, E>(awaitedResult.Error, awaitedResult.Message);

            var funcResult = await newFuncResult().ConfigureAwait(continueOnCapturedContext);
            if (funcResult.IsFailure)
                return Result.Failure<TOut, E>(funcResult.Error, funcResult.Message);

            return funcResult;
        }

        public static async Task<Result<TOut>> OnSuccessAsync<TIn, TOut>(
            this Task<Result<TIn>> result,
            Func<TIn, Task<Result<TOut>>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);

            return awaitedResult.IsSuccess
                ? await newFuncResult(awaitedResult.Value).ConfigureAwait(continueOnCapturedContext)
                : Result.Failure<TOut>();
        }

        public static async Task<Result<TOut, E>> OnSuccessAsync<TIn, TOut, E>(
            this Task<Result<TIn, E>> result,
            Func<TIn, Task<Result<TOut, E>>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);
            if (awaitedResult.IsFailure)
                return Result.Failure<TOut, E>(awaitedResult.Error, awaitedResult.Message);

            var funcResult = await newFuncResult(awaitedResult.Value).ConfigureAwait(continueOnCapturedContext);

            return funcResult;
        }

        public static async Task<Result<TOut>> OnSuccessAsync<TIn, TOut>(
            this Task<Result<TIn>> result,
            Func<TIn, Result<TOut>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);

            if (awaitedResult.IsSuccess)
                return newFuncResult(awaitedResult.Value);

            return Result.Failure<TOut>();
        }

        public static async Task<Result<T>> OnSuccessAsync<T>(
            this Task<Result<T>> result,
            Action<T> action,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);

            if (awaitedResult.IsSuccess)
                action(awaitedResult.Value);

            return awaitedResult;
        }

        public static async Task<Result<T, E>> OnBothAsync<T, E>(
            this Task<Result<T, E>> result,
            Func<Result<T, E>, Result<T, E>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);

            return newFuncResult(awaitedResult);
        }

        public static async Task<Result<T, E>> OnBothAsync<T, E>(
            this Task<Result<T, E>> result,
            Func<Result<T, E>, Task<Result<T, E>>> newFuncResult,
            bool continueOnCapturedContext = false)
        {
            var awaitedResult = await result.ConfigureAwait(continueOnCapturedContext);

            return await newFuncResult(awaitedResult).ConfigureAwait(continueOnCapturedContext);
        }
    }
}
