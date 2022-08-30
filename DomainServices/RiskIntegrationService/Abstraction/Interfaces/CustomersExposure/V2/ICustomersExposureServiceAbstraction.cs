namespace DomainServices.RiskIntegrationService.Abstraction.CustomersExposure.V2;

public interface ICustomersExposureServiceAbstraction
{
    /// <summary>
    /// Vrátí data související s angažovaností jednotlivých účastníků úvěrové žádosti(Loan Applicaiton).
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.CustomersExposure.V2.CustomersExposureCalculateResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<IServiceCallResult> Calculate(Contracts.CustomersExposure.V2.CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
