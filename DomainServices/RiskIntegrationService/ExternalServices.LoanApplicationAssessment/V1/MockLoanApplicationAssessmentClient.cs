using DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1;

internal sealed class MockLoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClient
{
    public Task<Identified> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken)
        => Task.FromResult(new Identified());
}
