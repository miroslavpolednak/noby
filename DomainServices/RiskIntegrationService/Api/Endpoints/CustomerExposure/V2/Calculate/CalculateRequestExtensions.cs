using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomerExposure.V2.Calculate;

internal static class CalculateRequestExtensions
{
    public static _C4M.LoanApplicationDealer ToC4mDealer(this ExternalServices.Dto.C4mUserInfoData userInfo, Contracts.Shared.Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
        => new()
        {
            Id = _C4M.ResourceIdentifier.Create(humanUser).ToC4M(),
            CompanyId = _C4M.ResourceIdentifier.Create(humanUser, userInfo.DealerCompanyId?.ToString()).ToC4M()
        };
#pragma warning restore CA1305 // Specify IFormatProvider
}
