using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

internal interface IRiskBusinessCaseClient
{
    Task<LoanApplicationCommit> CaseCommitment(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken);

    Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken);
}
