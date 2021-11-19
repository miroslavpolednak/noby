using Grpc.Core;
using System;
using Xunit;

namespace CIS.Infrastructure.gRPC.Tests
{
    public class GrpcErrorCollectionTest
    {
        public void CollectionIsNotEmpty_ShouldThrowException()
        {
            //arrange
            GrpcErrorCollection sut = new();
            sut.Add(1, "test");

            //act
            var ex = Assert.Throws<RpcException>(() => sut.ThrowExceptionIfErrors(StatusCode.Unknown, "test"));
        }

        public void CollectionIsEmpty_ShouldNotThrowException()
        {
            //arrange
            GrpcErrorCollection sut = new();

            //act
            bool result = sut.ThrowExceptionIfErrors(StatusCode.Unknown, "test");

            //assert
            Assert.True(result);
        }
    }
}
