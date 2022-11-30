namespace DomainServices.RiskIntegrationService.Clients.CreditWorthiness.V2;

public interface ICreditWorthinessServiceClient
{
    /// <summary>
    /// Výpočet rozšířené bonity
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.CreditWorthiness.V2.CreditWorthinessCalculateResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> Calculate(Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Výpočet rozšířené bonity
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.CreditWorthiness.V2.CreditWorthinessSimpleCalculateResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> SimpleCalculate(Contracts.CreditWorthiness.V2.CreditWorthinessSimpleCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
