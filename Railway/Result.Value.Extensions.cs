namespace Railway
{
    public static partial class ResultExtensions
	{
		public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
		{
			if (result.IsSuccess)
				action(result.Value);

			return result;
		}

		public static Result<T, E> OnSuccess<T, E>(this Result<T, E> result, Action<T> action)
		{
			if (result.IsSuccess)
				action(result.Value);

			return result;
		}

		public static Result<TOut> OnSuccess<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> newResultFunc)
		{
			if (result.IsSuccess)
				return newResultFunc(result.Value);

			return Result.Failure<TOut>();
		}

		public static Result<T> OnFailure<T>(this Result<T> result, Action action)
		{
			if (result.IsFailure)
				action();

			return result;
		}

        public static Result<T, E> OnFailure<T, E>(this Result<T, E> result, Action<E, string?> action)
		{
			if (result.IsFailure)
				action(result.Error, result.Message);

			return result;
        }

		public static Result<T> OnBoth<T>(this Result<T> result, Action<Result<T>> action)
		{
			action(result);

			return result;
		}

		public static Result<TOut> OnBoth<TIn, TOut>(this Result<TIn> result, Func<Result<TIn>, Result<TOut>> newResultFunc) => 
			newResultFunc(result);
	}
}
