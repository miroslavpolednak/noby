using DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V1;

internal sealed class MockRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{

    public Task<Contracts.RiskBusinessCase> CreateCase(Create request, CancellationToken cancellationToken)
        => Task.FromResult(new Contracts.RiskBusinessCase());

    public Task<Identified> CreateCaseAssessment(string riskBusinessCaseId, LoanApplicationAssessmentCreate request, CancellationToken cancellationToken)
        => Task.FromResult(new Identified());

    public Task<RiskBusinessCaseCommitCommandInstance> CreateCaseAssessmentAsynchronous(string riskBusinessCaseId, RiskBusinessCaseCommitCommand request, CancellationToken cancellationToken)
        => Task.FromResult(new RiskBusinessCaseCommitCommandInstance());

    public Task<RiskBusinessCaseCommit> CommitCase(string riskBusinessCaseId, RiskBusinessCaseCommitCreate request, CancellationToken cancellationToken)
        => Task.FromResult(new RiskBusinessCaseCommit
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
            RiskBusinessCaseId = "RBCID"
        });
}
