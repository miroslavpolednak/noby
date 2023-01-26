using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2;

public interface IRiskBusinessCaseServiceClient
{
    /// <summary>
    /// Založení úvěrového případu
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<RiskBusinessCaseCreateResponse> CreateCase(long salesArrangementId, string? resourceProcessId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Žádost o vyhodnocení úvěrové žádosti
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<LoanApplicationAssessmentResponse> CreateAssessment(RiskBusinessCaseCreateAssessmentRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání výsledků vyhodnocení
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<LoanApplicationAssessmentResponse> GetAssessment(RiskBusinessCaseGetAssessmentRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Založení úvěrového případu
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<RiskBusinessCaseCommitCaseResponse> CommitCase(RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
