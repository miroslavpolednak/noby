using CIS.Testing.Common;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class CaseMockExtensions
{
    public static void MockGetCaseDetail(this ICaseServiceClient caseService)
    {
        var fixture = FixtureFactory.Create();

        caseService.GetCaseDetail(Arg.Any<long>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(fixture.Create<Case>());
    }

    public static void MockValidateCaseId(this ICaseServiceClient caseService)
    {
        caseService.ValidateCaseId(Arg.Any<long>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new ValidateCaseIdResponse { Exists = true });
    }
}