using CIS.Security.InternalServices;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CIS.Security.Tests
{
    public class AuthHeaderParserTests
    {
        [Fact]
        public void ValidString_ShouldPass()
        {
            // arrange
            var sut = getSUT();

            // act
            var result = sut.Parse("Basic YTph");

            // assert
            Assert.True(result.Success);
            Assert.Equal("a", result.Login);
            Assert.Equal("a", result.Password);
        }

        [Fact]
        public void BearerAuthentication_ShouldFail()
        {
            // arrange
            var sut = getSUT();

            // act
            var result = sut.Parse("Bearer YTph");

            // assert
            Assert.False(result.Success);
            Assert.Equal(1, result.ErrorCode);
        }

        [Fact]
        public void InvalidHeader_ShouldFail()
        {
            // arrange
            var sut = getSUT();

            // act
            var result = sut.Parse("");

            // assert
            Assert.False(result.Success);
            Assert.Equal(3, result.ErrorCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Basic")]
        [InlineData("Basic a:a")]
        [InlineData("Basic notBase64")]
        public void InvalidString_ShouldFail(string content)
        {
            // arrange
            var sut = getSUT();

            // act
            var result = sut.Parse(content);

            // assert
            Assert.False(result.Success);
            Assert.Equal(3, result.ErrorCode);
        }

        [Fact]
        public void StringWithoutSemicolon_ShouldFail()
        {
            // arrange
            var sut = getSUT();

            // act
            var result = sut.Parse("Basic YWE=");

            // assert
            Assert.False(result.Success);
            Assert.Equal(2, result.ErrorCode);
        }

        private AuthHeaderParser getSUT()
        {
            var logger = Mock.Of<ILogger<IAuthHeaderParser>>();
            return new AuthHeaderParser(logger);
        }
    }
}
