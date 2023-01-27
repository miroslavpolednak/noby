using DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V1;

public interface ILoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClientBase
{
    Task<Identified> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken);

    const string Version = "V1";
}
