
namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplicationAssessment.V3;

public interface ILoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClientBase
{
    Task<Contracts.LoanApplicationAssessment> GetAssessment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken);

    const string Version = "V3";
}
