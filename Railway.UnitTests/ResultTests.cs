using Xunit;

namespace Railway.UnitTests
{
    public class ResultTests
    {
        private static readonly Result<string> SuccessV = Result.Success("Dune");
        private static readonly Result<string> FailureV = Result.Failure<string>();
        private static readonly Result<string, int> SuccessVE = Result.Success<string, int>("Arrakis");
        private static readonly Result<string, int> FailureVE = Result.Failure<string, int>(42, "Kwisatz Haderach");

        [Fact]
        public void Result_Test_IsSuccess()
        {
            var result = Result.Success();

            Assert.True(result.IsSuccess);
            Assert.NotEqual(result.IsFailure, result.IsSuccess);
        }

        [Fact]
        public void Result_Test_IsFailure()
        {
            var result = Result.Failure();

            Assert.True(result.IsFailure);
            Assert.NotEqual(result.IsSuccess, result.IsFailure);
        }

        [Fact]
        public void Result_Value_Test_IsSuccess()
        {
            Assert.True(SuccessV.IsSuccess);
            Assert.NotEqual(SuccessV.IsFailure, SuccessV.IsSuccess);
        }

        [Fact]
        public void Result_Value_Test_IsFailure()
        {
            Assert.True(FailureV.IsFailure);
            Assert.NotEqual(FailureV.IsSuccess, FailureV.IsFailure);
        }

        [Fact]
        public void Result_Value_AccessValue_DoesntThrow_OnSuccess()
        {
            Assert.NotNull(SuccessV.Value);
        }

        [Fact]
        public void Result_Value_AccessValue_Throws_OnFailure()
        {
            Func<string> test = () => FailureV.Value;

            Assert.Throws<InvalidOperationException>(test);
        }

        [Fact]
        public void Result_Value_Error_Test_IsSuccess()
        {
            Assert.True(SuccessVE.IsSuccess);
            Assert.NotEqual(SuccessVE.IsFailure, SuccessVE.IsSuccess);
        }

        [Fact]
        public void Result_Value_Error_Test_IsFailure()
        {
            Assert.True(FailureVE.IsFailure);
            Assert.NotEqual(FailureVE.IsSuccess, FailureVE.IsFailure);
        }

        [Fact]
        public void Result_Value_Error_AccessValue_DoesntThrow_OnSuccess()
        {
            Assert.NotNull(SuccessVE.Value);
        }

        [Fact]
        public void Result_Value_Error_AccessValue_Throws_OnFailure()
        {
            Func<string> test = () => FailureVE.Value;

            Assert.Throws<InvalidOperationException>(test);
        }

        [Fact]
        public void Result_Value_Error_AccessError_Throws_OnSuccess()
        {
            Func<int> test = () => SuccessVE.Error;

            Assert.Throws<InvalidOperationException>(() => test());
        }

        [Fact]
        public void Result_Value_Error_AccessMessage_Throws_OnSuccess()
        {
            Func<string> test = () => SuccessVE.Message;

            Assert.Throws<InvalidOperationException>(test);
        }

        [Fact]
        public void Result_Value_Error_AccessError_DoesntThrow_OnFailure()
        {
            Assert.NotEqual(0, FailureVE.Error);
        }

        [Fact]
        public void Result_Value_Error_AccessMessage_DoesntThrow_OnFailure()
        {
            Assert.NotNull(FailureVE.Message);
        }
    }
}
