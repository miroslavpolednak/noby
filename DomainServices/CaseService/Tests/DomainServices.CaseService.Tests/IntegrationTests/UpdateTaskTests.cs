using CIS.Testing;
using DomainServices.CaseService.Contracts;
using Grpc.Core;

namespace DomainServices.CaseService.Tests.IntegrationTests;
public class UpdateTaskTests : IntegrationTestBase
{
    public UpdateTaskTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
#pragma warning disable CA1707 // Identifiers should not contain underscores
    public async Task UpdateTask_WrongInputParameters_ShouldReturnExp()
#pragma warning restore CA1707 // Identifiers should not contain underscores
    {
        var request = createInvalidRequest();

        Func<Task> act = async () => await CreateGrpcClient().UpdateTaskAsync(request);

        await act.Should().ThrowAsync<RpcException>().Where(t => t.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact]
#pragma warning disable CA1707 // Identifiers should not contain underscores
    public async Task UpdateTask_CorrectInputParameters_ShouldReturnValidResponse()
#pragma warning restore CA1707 // Identifiers should not contain underscores
    {
        var request = createFullRequest();
        await CreateGrpcClient().UpdateTaskAsync(request);
    }

    private static UpdateTaskRequest createInvalidRequest()
    {
        return new()
        {
            CaseId = 1,
            TaskIdSb = 1,
            Retention = new()
            {
                FeeSum = 1
            }
        };
    }

    private static UpdateTaskRequest createFullRequest()
    {
        return new()
        {
            CaseId = 1,
            TaskIdSb = 1,
            Retention = new()
            {
                FeeSum = 1,
                FeeFinalSum = 1,
                InterestRateValidFrom = DateTime.Now.Date,
                LoanInterestRate = 1,
                LoanInterestRateProvided = 1,
                LoanPaymentAmount = 1,
                LoanPaymentAmountFinal = 1
            }
        };
    }
}
