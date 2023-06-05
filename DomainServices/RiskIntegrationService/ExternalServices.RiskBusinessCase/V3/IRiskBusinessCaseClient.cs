using DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3;

public interface IRiskBusinessCaseClient
    : IRiskBusinessCaseClientBase
{
    /// <summary>
    /// Vytvoření nového Risk Business Case
    /// </summary>
    Task<Contracts.RiskBusinessCase> CreateCase(Create request, CancellationToken cancellationToken);

    /// <summary>
    /// Vyhodnocení úvěrové žádosti
    /// </summary>
    Task<LoanApplicationAssessment> CreateCaseAssessment(string riskBusinessCaseId, LoanApplicationAssessmentCreate request, CancellationToken cancellationToken);

    /// <summary>
    /// Dokončení úvěrové žádosti
    /// </summary>
    Task<RiskBusinessCaseCommit> CommitCase(string riskBusinessCaseId, RiskBusinessCaseCommitCreate request, CancellationToken cancellationToken);

    const string Version = "V3";
}
