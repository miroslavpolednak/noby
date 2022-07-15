namespace DomainServices.RiskIntegrationService.Abstraction;

public interface ICreditWorthinessService
{
    /// <summary>
    /// Vraci detail Case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Case" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 13000; Case #{} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 13016; CaseId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> Calculate(Contracts.CreditWorthiness.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
