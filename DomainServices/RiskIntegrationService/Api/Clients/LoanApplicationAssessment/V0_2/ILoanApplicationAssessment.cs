using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V0_2.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V0_2;

internal interface ILoanApplicationAssessmentClient
{
    Task<Identified> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken);
}
