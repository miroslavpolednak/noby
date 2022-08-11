using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.Calculate;

internal static class CalculateResponseExtensions
{
    /*public static _V1.CustomersExposureCalculateResponse ToServiceResponse(this _C4M.LoanApplicationRelatedExposureResult response)
    {
        return new _V1.CustomersExposureCalculateResponse
        {
            ExposureSummary = response.ExposureSummary.Select(t => t.ToServiceResponse()).ToList()
        };
    }

    public static _V1.CustomersExposureSummary ToServiceResponse(this _C4M.ExposureSummaryForApproval item)
        => new _V1.CustomersExposureSummary
        {
            TotalExistingExposureKB = item.TotalExistingExposureKB,
            TotalExistingExposureKBNaturalPerson = item.TotalExistingExposureKBNonPurpose
        };*/
}
