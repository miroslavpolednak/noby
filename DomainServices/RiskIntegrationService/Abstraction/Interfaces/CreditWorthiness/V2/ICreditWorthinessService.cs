namespace DomainServices.RiskIntegrationService.Abstraction.CreditWorthiness.V2;

public interface ICreditWorthinessService
{
    /// <summary>
    /// Vraci detail Case
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Case" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> Calculate(Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
