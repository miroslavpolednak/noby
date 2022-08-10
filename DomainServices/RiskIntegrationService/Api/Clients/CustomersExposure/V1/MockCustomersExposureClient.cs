using DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1;

internal sealed class MockCustomersExposureClient
    : ICustomersExposureClient
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
