namespace Railway
{
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Only performs the action if the previous result succeeded. Returns the previous result
        /// </summary>
        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsSuccess)
                action();

            return result;
        }

        /// <summary>
        /// Calls the factory method provided, and returns its result if the previous result succeeded. Returns the failed result otherwise
        /// </summary>
        public static Result OnSuccess(this Result result, Func<Result> newResultFunc)
        {
            if (result.IsSuccess)
                return newResultFunc();

            return result;
        }

        /// <summary>
        /// Only performs the action if the previous result failed. Returns the previous result
        /// </summary>
        public static Result OnFailure(this Result result, Action action)
        {
            if (result.IsFailure)
                action();

            return result;
        }

        /// <summary>
        /// Calls the factory method provided, and returns its result if the previous result failed. Returns the successful result otherwise
        /// </summary>
        public static Result OnFailure(this Result result, Func<Result> newResultFunc)
        {
            if (result.IsFailure)
                return newResultFunc();

            return result;
        }

        /// <summary>
        /// Calls the provided action regardlesss of success of the previous result. Returns the previous result
        /// </summary>
        public static Result OnBoth(this Result result, Action action)
        {
            action();

            return result;
        }

        /// <summary>
        /// Calls the provided factory method regardlesss of success of the previous result. Returns the result of the factory method
        /// </summary>
        public static Result OnBoth(this Result _, Func<Result> newResultFunc) =>
            newResultFunc();
    }
}
