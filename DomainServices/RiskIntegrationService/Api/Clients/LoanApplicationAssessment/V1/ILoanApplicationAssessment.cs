using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V1;

internal interface ILoanApplicationAssessmentClient
{
    Task<Identified> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken);
}
