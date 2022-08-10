using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V1.Calculate;

internal static class CalculateRequestExtensions
{
    public static _C4M.LoanApplicationDealer ToC4mDealer(this Dto.C4mUserInfoData userInfo, Contracts.Shared.Identity humanUser)
        => new()
        {
            Id = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser),
            CompanyId = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString())
        };
}
