
namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V3;

internal sealed class MockLoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClient
{
    public Task<Contracts.LoanApplicationAssessment> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken)
        => Task.FromResult(new Contracts.LoanApplicationAssessment());
}
