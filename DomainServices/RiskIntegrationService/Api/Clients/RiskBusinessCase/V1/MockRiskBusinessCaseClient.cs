using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

internal sealed class MockRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{
#pragma warning disable CA1822 // Mark members as static
    public Task<LoanApplicationCommit> CaseCommitment(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken)
#pragma warning restore CA1822 // Mark members as static
    {
        return Task.FromResult<LoanApplicationCommit>(new LoanApplicationCommit
        {
            OperationResult = "vysledek",
            ResultReasons = new List<ResultReason>
            {
                new ResultReason
                {
                    Code = "1",
                    Desc = "2"
                }
            },
            Timestamp = DateTime.UtcNow,
            RiskBusinessCaseId = new ResourceIdentifier
            {
                Id = riskBusinessCaseId
            }
        });
    }

    public Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<LoanApplicationCreate>(new LoanApplicationCreate
        {
            RiskBusinessCaseId = new ResourceIdentifier
            {
                Id = "new-rbc-id"
            }
        });
    }
}
