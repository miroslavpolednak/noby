using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

namespace DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2;

public interface ICustomerExposureServiceClient
{
    /// <summary>
    /// Vrátí data související s angažovaností jednotlivých účastníků úvěrové žádosti(Loan Applicaiton).
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
    Task<CustomerExposureCalculateResponse> Calculate(CustomerExposureCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
