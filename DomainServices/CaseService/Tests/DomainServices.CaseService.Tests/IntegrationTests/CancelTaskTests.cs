using CIS.Testing;
using Grpc.Core;

namespace DomainServices.CaseService.Tests.IntegrationTests;

public class CancelTaskTests
    : IntegrationTestBase
{
    [Fact]
    public async Task RequestValidation_ShouldThrowRpcException()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () => await client.CancelTaskAsync(new Contracts.CancelTaskRequest
        {
            TaskIdSB = 0
        });

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task NonExistingTask_ShouldThrowSbWebapiException()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () => await client.CancelTaskAsync(new Contracts.CancelTaskRequest
        {
            TaskIdSB = 2
        });

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task ExistingTask_ShouldSucceed()
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () => await client.CancelTaskAsync(new Contracts.CancelTaskRequest
        {
            TaskIdSB = 1
        });

        await act.Should().NotThrowAsync();
    }

    public CancelTaskTests(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }
}
