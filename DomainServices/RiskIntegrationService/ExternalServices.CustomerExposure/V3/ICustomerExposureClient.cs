using DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3;

public interface ICustomerExposureClient
    : ICustomerExposureClientBase
{
    Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken);

    const string Version = "V3";
}