using DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1;

internal interface ICustomersExposureClient
{
    Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken);
}