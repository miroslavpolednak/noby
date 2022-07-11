using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

internal sealed class MockRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{
    public Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new LoanApplicationCreate
        {
            RiskBusinessCaseId = new ResourceIdentifier
            {
                Id = "new-rbc-id"
            }
        });

    public Task<Identified> CaseAssessment(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new Identified
        {
            
        });

    public Task<RiskBusinessCaseCommand> CaseAssessmentAsync(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new RiskBusinessCaseCommand
        {
            CommandId = 123456L
        });

    public Task<LoanApplicationCommit> CaseCommitment(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new LoanApplicationCommit
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
