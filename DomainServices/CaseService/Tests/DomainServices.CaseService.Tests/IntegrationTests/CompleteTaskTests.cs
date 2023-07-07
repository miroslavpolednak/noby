using CIS.Testing;
using Grpc.Core;
using Moq;

namespace DomainServices.CaseService.Tests.IntegrationTests;

public class CompleteTaskTests
    : IntegrationTestBase
{
    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 0)]
    public async Task RequestValidation_ShouldThrowRpcException(long caseId, int taskIdSb, int taskTypeId)
    {
        var client = CreateGrpcClient();

        Func<Task> act = async () => await client.CompleteTaskAsync(new Contracts.CompleteTaskRequest
        {
            CaseId = caseId,
            TaskIdSb = taskIdSb,
            TaskTypeId = taskTypeId
        });

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task ShouldSucceed()
    {
        var client = CreateGrpcClient();

        var result = await client.CompleteTaskAsync(new Contracts.CompleteTaskRequest
        {
            CaseId = 1,
            TaskIdSb = 1,
            TaskTypeId = 1,
            TaskResponseTypeId = 1,
            TaskUserResponse = ""
        });

        var sbRequest = new ExternalServices.SbWebApi.Dto.CompleteTask.CompleteTaskRequest 
        { 
            TaskIdSb = 1,
            Metadata = new Dictionary<string, string>
            {
                { "", "" }
            }
        };
    }

    public CompleteTaskTests(WebApplicationFactoryFixture<Program> fixture)
        : base(fixture)
    {
    }
}
