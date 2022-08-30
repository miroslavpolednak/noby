using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V1;

internal sealed class MockLoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClient
{
    public Task<Identified> GetAssesment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken)
        => Task.FromResult(new Identified());
}
