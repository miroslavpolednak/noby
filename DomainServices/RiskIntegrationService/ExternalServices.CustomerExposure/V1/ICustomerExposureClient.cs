using DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V1;

public interface ICustomerExposureClient
    : ICustomerExposureClientBase
{
    Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken);

    const string Version = "V1";
}