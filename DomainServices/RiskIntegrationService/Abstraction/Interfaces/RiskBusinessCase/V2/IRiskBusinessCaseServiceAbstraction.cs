namespace DomainServices.RiskIntegrationService.Abstraction.RiskBusinessCase.V2;

public interface IRiskBusinessCaseServiceAbstraction
{
    /// <summary>
    /// Založení úvěrového případu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.RiskBusinessCase.V2.RiskBusinessCaseCreateResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> CreateCase(Contracts.RiskBusinessCase.V2.RiskBusinessCaseCreateRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Žádost o vyhodnocení úvěrové žádosti
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.Shared.V1.LoanApplicationAssessmentResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> CreateAssesment(Contracts.RiskBusinessCase.V2.RiskBusinessCaseCreateAssesmentRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání výsledků vyhodnocení
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.Shared.V1.LoanApplicationAssessmentResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> GetAssesment(Contracts.RiskBusinessCase.V2.RiskBusinessCaseGetAssesmentRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Založení úvěrového případu
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.RiskBusinessCase.V2.RiskBusinessCaseCommitCaseResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> CommitCase(Contracts.RiskBusinessCase.V2.RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
