using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

internal interface IRiskBusinessCaseClient
{
    /// <summary>
    /// Vytvoření nového Risk Business Case
    /// </summary>
    Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Vyhodnocení úvěrové žádosti
    /// </summary>
    Task<Identified> CaseAssessment(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Vyhodnocení úvěrové žádosti - asynchronní
    /// </summary>
    Task<RiskBusinessCaseCommand> CaseAssessmentAsync(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Dokončení úvěrové žádosti
    /// </summary>
    Task<LoanApplicationCommit> CaseCommitment(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken);
}
