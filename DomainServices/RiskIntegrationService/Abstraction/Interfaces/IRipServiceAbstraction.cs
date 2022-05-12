using CIS.Core.Results;

namespace DomainServices.RiskIntegrationService.Abstraction;

public interface IRipServiceAbstraction
{
    /// <summary>
    /// Vypocet bonity
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.CreditWorthinessRequest" /></returns>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> CreditWorthiness(Contracts.CreditWorthinessRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
