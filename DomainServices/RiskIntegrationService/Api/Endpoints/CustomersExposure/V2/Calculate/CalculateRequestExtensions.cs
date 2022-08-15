using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.Calculate;

internal static class CalculateRequestExtensions
{
    public static _C4M.LoanApplicationDealer ToC4mDealer(this Dto.C4mUserInfoData userInfo, Contracts.Shared.Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
        => new()
        {
            Id = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser),
            CompanyId = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString())
        };
#pragma warning restore CA1305 // Specify IFormatProvider
}
