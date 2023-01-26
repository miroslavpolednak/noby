using DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1;

public interface ICustomersExposureClient
    : ICustomersExposureClientBase
{
    Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken);

    const string Version = "V1";
}