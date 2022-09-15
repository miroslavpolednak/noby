using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssessmentAsynchronous;

internal static class CreateAssessmentAsynchronousRequestExtensions
{
    public static _C4M.RiskBusinessCaseCommitCommand ToC4M(this _V2.RiskBusinessCaseCreateAssessmentAsynchronousRequest request, string chanel)
        => throw new NotImplementedException();
}
