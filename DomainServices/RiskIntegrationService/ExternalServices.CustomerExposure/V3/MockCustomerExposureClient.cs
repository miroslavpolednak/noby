using DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3;

internal sealed class MockCustomerExposureClient
    : ICustomerExposureClient
{
    public Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken)
    {
        return Task.FromResult<LoanApplicationRelatedExposureResult>(new LoanApplicationRelatedExposureResult{});
    }
}
