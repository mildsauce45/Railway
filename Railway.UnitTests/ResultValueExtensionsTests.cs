using System.Drawing;
using Xunit;

namespace Railway.UnitTests
{
    public class ResultValueExtensionsTests
    {
        [Fact]
        public void OnSuccess_Value_CallsAction_WhenSuccessful()
        {
            var list = new List<int>();

            Assert.True(Result.Success(42).OnSuccess(list.Add).IsSuccess);
            Assert.Single(list);
        }

        [Fact]
        public void OnSuccess_Value_DoesNot_CallAction_WhenFailure()
        {
            var list = new List<int>();

            Assert.True(Result.Failure<int>().OnSuccess(list.Add).IsFailure);
            Assert.Empty(list);
        }

        [Fact]
        public void OnSuccess_Value_Error_CallsAction_WhenSuccessful()
        {
            var list = new List<int>();

            Assert.True(Result.Success<int, ArgumentNullException>(42).OnSuccess(list.Add).IsSuccess);
            Assert.Single(list);
        }

        [Fact]
        public void OnSuccess_Value_Error_DoesNot_CallAction_WhenFailure()
        {
            var list = new List<int>();

            Assert.True(Result.Failure<int, ArgumentNullException>(new ArgumentNullException(), "testing").OnSuccess(list.Add).IsFailure);
            Assert.Empty(list);
        }

        [Fact]
        public void OnSuccess_Value_CallsResultFactory_WhenSuccessful()
        {
            var expected = "int";

            var result = Result.Success(42).OnSuccess(_ => Result.Success("int"));

            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void OnSuccess_Value_DoesNot_CallResultFactory_WhenFailure() =>
            Assert.True(Result.Failure<int>().OnSuccess(_ => Result.Success("string")).IsFailure);

        [Fact]
        public void OnFailure_Value_CallsAction_WhenFailure()
        {
            var list = new List<int>();

            Assert.True(Result.Failure<string>().OnFailure(() => list.Add(42)).IsFailure);
            Assert.Single(list);
        }

        [Fact]
        public void OnFailure_Value_DoesNot_CallAction_WhenSuccess()
        {
            var list = new List<int>();

            Assert.True(Result.Success("dune").OnFailure(() => list.Add(42)).IsSuccess);
            Assert.Empty(list);
        }

        [Fact]
        public void OnFailure_Value_Error_CallsAction_WhenFailure()
        {
            var point = Point.Empty;
            var message = string.Empty;

            Assert.True(Result.Failure<int, Point>(new Point(1, 1), "testing").OnFailure((p, m) => { point = p; message = m; }).IsFailure);
            Assert.Equal(new Point(1, 1), point);
            Assert.False(string.IsNullOrWhiteSpace(message));
            Assert.Equal("testing", message);
        }

        [Fact]
        public void OnFailure_Value_Error_DoesNot_CallAction_WhenSuccess()
        {
            var point = Point.Empty;
            var message = string.Empty;

            Assert.True(Result.Success<int, Point>(42).OnFailure((p, m) => { point = p; message = m; }).IsSuccess);
            Assert.Equal(Point.Empty, point);
            Assert.True(string.IsNullOrWhiteSpace(message));
        }

        [Fact]
        public void OnBoth_Value_CallsAction_WhenSuccess()
        {
            var list = new List<int>();

            Assert.True(Result.Success(42).OnBoth(r => list.Add(r.Value)).IsSuccess);
            Assert.Single(list);
        }

        [Fact]
        public void OnBoth_Value_CallsAction_WhenFailure()
        {
            var list = new List<int>();

            Assert.True(Result.Failure<int>().OnBoth(r => list.Add(42)).IsFailure);
            Assert.Single(list);
        }

        [Fact]
        public void OnBoth_Value_CallsResultFactory_WhenSuccess()
        {
            var result = Result.Success(42)
                .OnBoth(r => Result.Success(r.Value.ToString()));

            Assert.True(result.IsSuccess);
            Assert.Equal("42", result.Value);
        }

        [Fact]
        public void OnBoth_Value_CallsResultFactory_WhenFailure()
        {
            var result = Result.Failure<int>()
                .OnBoth(r => Result.Success("42"));

            Assert.True(result.IsSuccess);
            Assert.Equal("42", result.Value);
        }
    }
}
