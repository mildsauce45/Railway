using Xunit;

namespace Railway.UnitTests
{
    public class ResultTaskExtensionsTests
    {
        private const string TestErrorMessage = "testing";
        private const int TestStartingIntValue = 42;

        [Fact]
        public async Task OnSuccessAsync_CallsFactory_Task_To_Task()
        {
            var finalResult = await GetFirstStep()
                .OnSuccessAsync(GetSecondStep)
                .OnSuccessAsync(GetThirdStep);

            Assert.True(finalResult.IsSuccess);
            Assert.Equal(127, finalResult.Value);
        }

        [Fact]
        public async Task OnSuccessAsync_CallsFactory_NonTask_To_Task()
        {
            var finalResult = await Result.Success(20)
                .OnSuccessAsync(GetThirdStep);

            Assert.True(finalResult.IsSuccess);
            Assert.Equal(61, finalResult.Value);
        }

        [Fact]
        public async Task OnSuccessAsync_DoesNotCallFactory_NonTask_To_Task()
        {
            var factoryCalled = false;

            var finalResult = await GivenFailedNonTaskInt()
                .OnSuccessAsync(i => { factoryCalled = true; return Task.FromResult(Result.Success(i)); });

            Assert.True(finalResult.IsFailure);
            Assert.False(factoryCalled);
        }

        [Fact]
        public async Task OnSuccessAsync_CallsFactory_NonTask_To_Task_PostAction()
        {
            var localValue = 0;

            await Result.Success(20)
                .OnSuccessAsync(GetThirdStep, x => localValue = x);

            Assert.Equal(61, localValue);
        }

        [Fact]
        public async Task OnSuccessAsync_DoesNotCallFactory_NonTask_To_Task_PostAction()
        {
            var localValue = 0;

            var finalResult = await GivenFailedNonTaskInt()
                .OnSuccessAsync(GetThirdStep, x => localValue = x);

            Assert.True(finalResult.IsFailure);
            Assert.Equal(0, localValue);
        }

        [Fact]
        public async Task OnSuccessAsync_CallsFactory_NonTask_To_Task_PostAction_HasErrorType()
        {
            var localValue = 0;

            var finalResult = await GivenSuccessNonTaskInt()
                .OnSuccessAsync(GivenSuccessfulDoubleIntTaskWithErrorType, x => localValue = x);

            Assert.True(finalResult.IsSuccess);
            Assert.Equal(localValue, finalResult.Value);
            Assert.Equal(TestStartingIntValue * 2, localValue);
        }

        [Fact]
        public async Task OnSuccessAsync_DoesNotCallFactory_NonTask_To_Task_PostAction_HasErrorType()
        {
            var localValue = 0;

            var finalResult = await GivenFailedNonTaskInt()
                .OnSuccessAsync(GivenSuccessfulDoubleIntTaskWithErrorType, x => localValue = x);

            Assert.True(finalResult.IsFailure);
            Assert.Equal("Converted failed Result<Int32> to Result<Int32,String>", finalResult.Message);
            Assert.Equal(0, localValue);
        }

        [Fact]
        public async Task OnSuccessAsync_CallsFactory_NonTask_To_Task_DoesntCallPostAction_HasErrorType()
        {
            var localValue = 0;

            var finalResult = await GivenSuccessNonTaskInt()
                .OnSuccessAsync(GivenFailedIntTaskWithErrorType, x => localValue = x);

            Assert.True(finalResult.IsFailure);
            Assert.Equal("ERROR", finalResult.Error);
            Assert.Equal(TestErrorMessage, finalResult.Message);
            Assert.Equal(0, localValue);
        }

        [Fact]
        public async Task OnSuccessAsync_CallsFactory_Task_To_NonTask()
        {
            var finalResult = await GetFirstStep()
                .OnSuccessAsync(s => Result.Success(s + "37"));

            Assert.True(finalResult.IsSuccess);
            Assert.Equal("4237", finalResult.Value);
        }

        [Fact]
        public async Task OnSuccessAsync_CallsAction()
        {
            var transformed = string.Empty;

            var finalResult = await GetFirstStep()
                .OnSuccessAsync(s => transformed = s[..1])
                .OnSuccessAsync(GetSecondStep);

            Assert.True(finalResult.IsSuccess);
            Assert.Equal(42, finalResult.Value);
            Assert.Equal("4", transformed);
        }

        [Fact]
        public async Task OnSuccessAsync_Value_Error_Task_CallsAction_WhenSuccess()
        {
            var list = new List<int>();

            var result = await Task.FromResult(Result.Success<int, object>(TestStartingIntValue))
                .OnSuccessAsync(list.Add);

            Assert.True(result.IsSuccess);
            Assert.Single(list);
        }

        [Fact]
        public async Task OnSuccessAsync_Value_Error_Task_DoesNot_CallAction_WhenFailure()
        {
            var list = new List<int>();

            var result = await Task.FromResult(Result.Failure<int, object>(new object(), TestErrorMessage))
                .OnSuccessAsync(list.Add);

            Assert.True(result.IsFailure);
            Assert.Empty(list);
        }

        [Fact]
        public async Task OnSuccessAsync_Value_Error_Task_CallsTaskResultFactory_WhenSuccess()
        {
            var result = await Task.FromResult(Result.Success<int, object>(TestStartingIntValue))
                .OnSuccessAsync(i => Task.FromResult(Result.Success<string, object>(i.ToString())));

            Assert.True(result.IsSuccess);
            Assert.Equal($"{TestStartingIntValue}", result.Value);
        }

        [Fact]
        public async Task OnSuccessAsync_Value_Error_Task_DoesNot_CallTaskResultFactory_WhenFailure()
        {
            var result = await Task.FromResult(Result.Failure<int, object>(new object(), TestErrorMessage))
                .OnSuccessAsync(i => Task.FromResult(Result.Success<string, object>(i.ToString())));

            Assert.True(result.IsFailure);
            Assert.Equal(TestErrorMessage, result.Message);
        }

        [Fact]
        public async Task OnBothAsync_Value_Error_Task_CallsFactory_WhenSuccess()
        {
            bool invoked = false;

            var result = await Task.FromResult(Result.Success<int, object>(TestStartingIntValue))
                .OnBothAsync(r =>
                {
                    invoked = true;
                    return r;
                });

            Assert.True(result.IsSuccess);
            Assert.Equal(TestStartingIntValue, result.Value);
            Assert.True(invoked);
        }

        [Fact]
        public async Task OnBothAsync_Value_Error_Task_CallsFactory_WhenFailure()
        {
            bool invoked = false;

            var result = await Task.FromResult(Result.Failure<int, object>(new object(), TestErrorMessage))
                .OnBothAsync(r =>
                {
                    invoked = true;
                    return r;
                });

            Assert.True(result.IsFailure);
            Assert.Equal(TestErrorMessage, result.Message);
            Assert.True(invoked);
        }

        [Fact]
        public async Task OnBothAsync_Value_Error_Task_CallsTaskFactory_WhenSuccess()
        {
            bool invoked = false;

            var result = await Task.FromResult(Result.Success<int, object>(TestStartingIntValue))
                .OnBothAsync(async r =>
                {
                    await Task.Delay(1);
                    invoked = true;
                    return r;
                });

            Assert.True(result.IsSuccess);
            Assert.Equal(TestStartingIntValue, result.Value);
            Assert.True(invoked);
        }

        [Fact]
        public async Task OnBothAsync_Value_Error_Task_CallsTaskFactory_WhenFailure()
        {
            bool invoked = false;

            var result = await Task.FromResult(Result.Failure<int, object>(new object(), TestErrorMessage))
                .OnBothAsync(async r =>
                {
                    await Task.Delay(1);
                    invoked = true;
                    return r;
                });

            Assert.True(result.IsFailure);
            Assert.Equal(TestErrorMessage, result.Message);
            Assert.True(invoked);
        }

        private static Task<Result<string>> GetFirstStep() =>
            Task.FromResult(Result.Success("42"));

        private static Task<Result<int>> GetSecondStep(string s) =>
            Task.FromResult(Result.Success(int.Parse(s)));

        private static Task<Result<int>> GetThirdStep(int x) =>
            Task.FromResult(Result.Success(x * 3 + 1));

        private static Task<Result<int, string>> GivenSuccessfulDoubleIntTaskWithErrorType(int x) =>
            Task.FromResult(Result.Success<int, string>(x * 2));

        private static Task<Result<int, string>> GivenFailedIntTaskWithErrorType(int _) =>
            Task.FromResult(Result.Failure<int, string>("ERROR", TestErrorMessage));

        private static Result<int> GivenSuccessNonTaskInt() =>
            Result.Success(TestStartingIntValue);

        private static Result<int> GivenFailedNonTaskInt() =>
            Result.Failure<int>();
    }
}
