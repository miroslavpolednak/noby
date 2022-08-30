using DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2;

internal interface IRiskBusinessCaseClient
{
    /// <summary>
    /// Vytvoření nového Risk Business Case
    /// </summary>
    Task<LoanApplicationCreate> CreateCase(CreateRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Vyhodnocení úvěrové žádosti
    /// </summary>
    Task<Identified> CreateCaseAssessment(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Vyhodnocení úvěrové žádosti - asynchronní
    /// </summary>
    Task<RiskBusinessCaseCommand> CreateCaseAssessmentAsynchronous(string riskBusinessCaseId, AssessmentRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Dokončení úvěrové žádosti
    /// </summary>
    Task<LoanApplicationCommit> CommitCase(string riskBusinessCaseId, CommitRequest request, CancellationToken cancellationToken);
}
