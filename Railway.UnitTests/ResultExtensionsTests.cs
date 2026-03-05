using Xunit;

namespace Railway.UnitTests
{
    public class ResultExtensionsTests
    {
        private static readonly Result Success = Result.Success();
        private static readonly Result Failure = Result.Failure();

        private bool _invoked;

        public ResultExtensionsTests()
        {
            _invoked = false;
        }

        [Fact]
        public void OnSuccess_CallsAction_WhenResultSuccessful()
        {
            Success
                .OnSuccess(SetInvoked);

            Assert.True(_invoked);
        }

        [Fact]
        public void OnSuccess_DoesNotCallAction_WhenResultFailure()
        {
            Failure
                .OnSuccess(SetInvoked);

            Assert.False(_invoked);
        }

        [Fact]
        public void OnFailure_CallsAction_WhenResultFailure()
        {
            Failure
                .OnFailure(SetInvoked);

            Assert.True(_invoked);
        }

        [Fact]
        public void OnFailure_DoesNotCallsAction_WhenResultSuccess()
        {
            Success
                .OnFailure(SetInvoked);

            Assert.False(_invoked);
        }

        [Fact]
        public void OnSuccess_CallsFactory_WhenResultSuccess() =>
            Assert.True(
                Success.OnSuccess(() => Failure).IsFailure,
                "because the factory should be returning a failed result");

        [Fact]
        public void OnSuccess_DoesNotCallFactory_WhenResultFailure() =>
            Assert.True(
                Failure.OnSuccess(() => Success).IsFailure,
                "becase we're not invoking the factory, and should instead be returning the original");

        [Fact]
        public void OnFailure_CallsFactory_WhenResultFailure() =>
            Assert.True(
                Failure.OnFailure(() => Success).IsSuccess,
                "because the factory should be returning a successful result");

        [Fact]
        public void OnFailure_DoesNotCallFactory_WhenResultSuccess() =>
            Assert.True(
                Success.OnFailure(() => Failure).IsSuccess,
                "becase we're not invoking the factory, and should instead be returning the original");

        [Fact]
        public void OnBoth_CallsAction_NoMatterWhat()
        {
            var count = 0;

            Success.OnBoth(() => count++);
            Failure.OnBoth(() => count++);

            Assert.Equal(2, count);
        }

        [Fact]
        public void OnBothCallsFactory_NoMatterWhat()
        {
            var successOnBoth = Success.OnBoth(() => Failure);
            var failureOnBoth = Failure.OnBoth(() => Success);

            Assert.Equal(Failure, successOnBoth);
            Assert.Equal(Success, failureOnBoth);
        }

        private void SetInvoked() => _invoked = true;
    }
}
