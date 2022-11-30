using DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1;

internal interface ICustomersExposureClient
{
    Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken);
}