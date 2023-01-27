using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Clients.CreditWorthiness.V2;

public interface ICreditWorthinessServiceClient
{
    /// <summary>
    /// Výpočet rozšířené bonity
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<CreditWorthinessCalculateResponse> Calculate(CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Výpočet rozšířené bonity
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<CreditWorthinessSimpleCalculateResponse> SimpleCalculate(CreditWorthinessSimpleCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
