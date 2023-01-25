using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1;

internal interface ILoanApplicationAssessmentClient
    : IExternalServiceClient
{
    Task<Identified> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken);

    const string Version = "V1";
}
