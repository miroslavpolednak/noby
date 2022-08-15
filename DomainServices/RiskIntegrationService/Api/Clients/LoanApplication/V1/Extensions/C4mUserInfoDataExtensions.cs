using DomainServices.RiskIntegrationService.Api.Dto;
using DomainServices.RiskIntegrationService.Contracts.Shared;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

internal static class C4mUserInfoDataExtensions
{
    public static LoanApplicationDealer ToC4mDealer(this C4mUserInfoData userInfo, Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
    => new()
    {
        Id = ResourceIdentifier.Create("BM", "Broker", humanUser),
        CompanyId = ResourceIdentifier.Create("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString())
    };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static KBGroupPerson ToC4mPerson(this C4mUserInfoData userInfo, Identity humanUser)
        => new()
        {
            Id = ResourceIdentifier.Create("PM", "KBGroupPerson", humanUser),
            Surname = userInfo.PersonSurname,
            OrgUnit = new OrgUnit
            {
                Id = userInfo.DealerCompanyId.ToString(),
                JobPost = new JobPost
                {
                    Id = userInfo.PersonJobPostId
                },
                Name = userInfo.PersonOrgUnitName
            }
        };
}
