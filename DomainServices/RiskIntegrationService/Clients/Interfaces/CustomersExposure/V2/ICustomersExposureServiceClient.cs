using DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

namespace DomainServices.RiskIntegrationService.Clients.CustomersExposure.V2;

public interface ICustomersExposureServiceClient
{
    /// <summary>
    /// Vrátí data související s angažovaností jednotlivých účastníků úvěrové žádosti(Loan Applicaiton).
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<CustomersExposureCalculateResponse> Calculate(CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
