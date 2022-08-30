using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2;

internal sealed class MockRiskBusinessCaseClient
    : IRiskBusinessCaseClient
{

    public Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new LoanApplicationCreate());

    public Task<Identified> CreateCaseAssessment(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new Identified
        {

        });

    public Task<RiskBusinessCaseCommand> CreateCaseAssessmentAsynchronous(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new RiskBusinessCaseCommand
        {
            CommandId = 123456L
        });

    public Task<LoanApplicationCommit> CommitCase(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken)
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
            RiskBusinessCaseId = "RBCID"
        });
}
