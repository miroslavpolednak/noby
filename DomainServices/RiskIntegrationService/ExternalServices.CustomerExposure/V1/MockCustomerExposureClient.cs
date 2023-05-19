using DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V1;

internal sealed class MockCustomerExposureClient
    : ICustomerExposureClient
{
    public Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken)
    {
        return Task.FromResult<LoanApplicationRelatedExposureResult>(new LoanApplicationRelatedExposureResult
        {
            ExposureSummary = new List<ExposureSummaryForApproval>
            {
                new ExposureSummaryForApproval
                {
                    TotalExistingExposureKB = null,
                    TotalExistingExposureKBNaturalPerson = null
                }
            }
        });
    }
}
