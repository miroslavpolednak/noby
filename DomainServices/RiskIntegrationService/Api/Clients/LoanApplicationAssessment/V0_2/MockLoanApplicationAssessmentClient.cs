using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V0_2.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V0_2;

internal sealed class MockLoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClient
{
    public Task<Identified> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken)
        => Task.FromResult(new Identified());
}
