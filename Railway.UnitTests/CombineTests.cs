using Xunit;

namespace Railway.UnitTests
{
    public class CombineTests
    {
        [Fact]
        public void Combine_Results_Handles_Success()
        {
            var success1 = Result.Success();
            var success2 = Result.Success();

            Assert.True(Combine.Results([success1, success2]).IsSuccess);
        }

        [Fact]
        public void Combine_Results_Handles_Failure()
        {
            var success = Result.Success();
            var failure = Result.Failure();

            Assert.True(Combine.Results([success, failure]).IsFailure);
        }

        [Fact]
        public async Task Combine_Results_Tasks_Handles_Success()
        {
            var success1 = Task.FromResult(Result.Success());
            var success2 = Task.FromResult(Result.Success());

            Assert.True((await Combine.ResultsAsync([success1, success2])).IsSuccess);
        }

        [Fact]
        public async Task Combine_Results_Tasks_Handles_Failure()
        {
            var success = Task.FromResult(Result.Success());
            var failure = Task.FromResult(Result.Failure());

            Assert.True((await Combine.ResultsAsync([success, failure])).IsFailure);
        }

        [Fact]
        public async Task Combine_Results_TaskFactories_Handles_Success()
        {
            Func<Task<Result>> success1 = () => Task.FromResult(Result.Success());
            Func<Task<Result>> success2 = () => Task.FromResult(Result.Success());

            Assert.True((await Combine.ResultsAsync([success1, success2])).IsSuccess);
        }

        [Fact]
        public async Task Combine_Results_TaskFactories_Handles_Failure()
        {
            Func<Task<Result>> success = () => Task.FromResult(Result.Success());
            Func<Task<Result>> failure = () => Task.FromResult(Result.Failure());

            Assert.True((await Combine.ResultsAsync([success, failure])).IsFailure);
        }

        [Fact]
        public void Combine_Results_Value_Handles_Success()
        {
            var success1 = Result.Success("Paul");
            var success2 = Result.Success("Atreides");

            Assert.True(Combine.Results([success1, success2]).IsSuccess);
        }

        [Fact]
        public void Combine_Results_Value_Handles_Failure()
        {
            var success = Result.Success("Paul");
            var failure = Result.Failure<string>();

            Assert.True(Combine.Results([success, failure]).IsFailure);
        }

        [Fact]
        public async Task Combine_Results_Value_Tasks_Handles_Success()
        {
            var success1 = Task.FromResult(Result.Success("Baron"));
            var success2 = Task.FromResult(Result.Success("Harkonnen"));

            Assert.True((await Combine.ResultsAsync([success1, success2])).IsSuccess);
        }

        [Fact]
        public async Task Combine_Results_Value_Tasks_Handles_Failure()
        {
            var success = Task.FromResult(Result.Success("Baron"));
            var failure = Task.FromResult(Result.Failure<string>());

            Assert.True((await Combine.ResultsAsync([success, failure])).IsFailure);
        }

        [Fact]
        public async Task Combine_Results_Value_TaskFactories_Handles_Success()
        {
            Func<Task<Result<string>>> success1 = () => Task.FromResult(Result.Success("Kwisatz"));
            Func<Task<Result<string>>> success2 = () => Task.FromResult(Result.Success("Haderach"));

            Assert.True((await Combine.ResultsAsync([success1, success2])).IsSuccess);
        }

        [Fact]
        public async Task Combine_Results_Value_TaskFactories_Handles_Failure()
        {
            Func<Task<Result<string>>> success = () => Task.FromResult(Result.Success("Kwisatz"));
            Func<Task<Result<string>>> failure = () => Task.FromResult(Result.Failure<string>());

            Assert.True((await Combine.ResultsAsync([success, failure])).IsFailure);
        }

        [Fact]
        public void Combine_Results_Value_Error_Handles_Success()
        {
            var success1 = Result.Success<string, int>("Paul");
            var success2 = Result.Success<string, int>("Atreides");

            Assert.True(Combine.Results([success1, success2], _ => Result.Success<long, int>(1), 0).IsSuccess);
        }

        [Fact]
        public void Combine_Results_Value_Error_Handles_Failure()
        {
            var success = Result.Success<string, int>("Paul");
            var failure1 = Result.Failure<string, int>(0, "testing1");
            var failure2 = Result.Failure<string, int>(0, "testing2");

            var combinedResult = Combine.Results([success, failure1, failure2], _ => Result.Success<long, int>(1), 1);

            Assert.True(combinedResult.IsFailure);
            Assert.Equal(1, combinedResult.Error);
            Assert.Equal("testing1, testing2", combinedResult.Message);
        }

        [Fact]
        public void Combine_Results_Value_Error_NoSuccessFactory_Handles_Success()
        {
            var success1 = Result.Success<string, int>("Paul");
            var success2 = Result.Success<string, int>("Atreides");

            var combinedResult = Combine.Results([success1, success2], 0);

            Assert.True(combinedResult.IsSuccess);
            Assert.True(combinedResult.Value);
        }

        [Fact]
        public void Combine_Results_Value_Error_NoSuccessFactory_Handles_Failure()
        {
            var success = Result.Success<string, int>("Paul");
            var failure = Result.Failure<string, int>(42, "testing");

            var combinedResult = Combine.Results([success, failure], 0);

            Assert.True(combinedResult.IsFailure);
            Assert.Equal(0, combinedResult.Error);
            Assert.Equal("testing", combinedResult.Message);
        }

        [Fact]
        public void Combine_Results_Value_Error_ProvidedSuccess_Handles_Success()
        {
            var success1 = Result.Success<string, int>("Paul");
            var success2 = Result.Success<string, int>("Atreides");

            var combinedResult = Combine.Results([success1, success2], "Success", 0);

            Assert.True(combinedResult.IsSuccess);
            Assert.Equal("Success", combinedResult.Value);
        }

        [Fact]
        public void Combine_Results_Value_Error_ProvidedSuccess_Handles_Failure()
        {
            var success = Result.Success<string, int>("Paul");
            var failure = Result.Failure<string, int>(42, "testing");

            var combinedResult = Combine.Results([success, failure], "Success", 0);

            Assert.True(combinedResult.IsFailure);
            Assert.Equal(0, combinedResult.Error);
            Assert.Equal("testing", combinedResult.Message);
        }
    }
}
