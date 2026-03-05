namespace Railway
{

    // Possible TODO: Move the Message property into both Result and Result<T> instead of just Result<T, E>

    public class Result : IResult
    {
        private readonly bool _isSuccess;

        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !_isSuccess;

        internal Result(bool isSuccess) =>
            _isSuccess = isSuccess;

        public static Result Success() => new(true);

        public static Result<T> Success<T>(T value) => new(value);

        public static Result<T, E> Success<T, E>(T value) => new(value);

        public static Result Failure() => new(false);

        public static Result<T> Failure<T>() => new();

        public static Result<T, E> Failure<T, E>(E error, string message) => new(error, message);
    }

    public class Result<T> : IResult, IValue<T>
    {
        private readonly T _value;
        private readonly bool _isSuccess;

        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !IsSuccess;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Cannot access Value if the result is not a success");

                return _value;
            }
        }

        internal Result(T value)
        {
            _isSuccess = true;
            _value = value;
        }

        internal Result()
        {
            _value = default!;
            _isSuccess = false;
        }
    }

    public class Result<T, E> : IResult<T, E>
    {
        private readonly T _value;
        private readonly E _error;
        private readonly bool _isSuccess;
        private readonly string _message;

        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !IsSuccess;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Cannot access Value if the result is not a success");

                return _value;
            }
        }

        public E Error
        {
            get
            {
                if (!IsFailure)
                    throw new InvalidOperationException("Cannot access Error if the result is a success");

                return _error;
            }
        }

        public string Message
        {
            get
            {
                if (!IsFailure)
                    throw new InvalidOperationException("Cannot access Message if the result is a success");

                return _message;
            }
        }

        internal Result(T value)
        {
            _isSuccess = true;
            _value = value;
            _message = default!;

            _error = default!;
        }

        internal Result(E error, string message)
        {
            _isSuccess = false;
            _message = message;
            _error = error;

            _value = default!;
        }
    }
}
