using CIS.Core.Exceptions;
using Grpc.Core;
using System;
using Xunit;

namespace CIS.Infrastructure.gRPC.Tests
{
    public class GrpcExceptionHelpersTests
    {
        [Theory]
        [InlineData("test message")]
        [InlineData("")]
        public void ParseMessageFromRpcException_ShouldReturnMessage(string msg)
        {
            //arrange
            var sut = new RpcException(new Status(), msg);

            //act
            var message = GrpcExceptionHelpers.GetErrorMessageFromRpcException(sut);

            //assert
            Assert.Equal(msg, message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(999999)]
        public void GetExceptionCodeFromTrailers_ShouldReturnCode(int code)
        {
            //arrange
            var sut = new RpcException(new Status(), new Metadata()
            {
                new(ExceptionHandlingConstants.GrpcTrailerCisCodeKey, code.ToString())
            });

            //act
            var returnedCode = GrpcExceptionHelpers.GetExceptionCodeFromTrailers(sut);

            //assert
            Assert.Equal(code, returnedCode);
        }

        [Theory]
        [InlineData(1, "test message", "my_param")]
        [InlineData(9999999, "test message", "my param")]
        public void CreateArgumentRpcException_ShouldReturnCorrectTrailers(int code, string message, string paramName)
        {
            //arrange
            var sut = GrpcExceptionHelpers.CreateArgumentRpcException(message, code, paramName);

            //assert
            Assert.Equal(code, Convert.ToInt32(sut.Trailers.GetValue(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)));
            Assert.Equal(paramName, sut.Trailers.GetValue(ExceptionHandlingConstants.GrpcTrailerCisArgumentKey));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateArgumentRpcExceptionWithEmptyParam_ShouldThrowException(string paramName)
        {
            //act
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() => GrpcExceptionHelpers.CreateArgumentRpcException("test message", 1, paramName));

            //assert
            Assert.Equal("paramName is empty (Parameter 'paramName')", sut.Message);
            Assert.Equal("paramName", sut.ParamName);
        }

        [Fact]
        public void CreateArgumentRpcExceptionWithIncorrectCode_ShouldThrowException()
        {
            //act
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() => GrpcExceptionHelpers.CreateArgumentRpcException("", 0, "my_param"));

            //assert
            Assert.Equal("exceptionCode <= 0 (Parameter 'exceptionCode')", sut.Message);
        }
    }
}
