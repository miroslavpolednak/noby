using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.RiskIntegrationService.ExternalServices.Dto;

namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;

public static class C4mUserInfoDataExtensions
{
    public static LoanApplicationDealer ToC4mDealer(this C4mUserInfoData userInfo, Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
    => new()
    {
        Id = ResourceIdentifier.Create("BM", "Broker", humanUser).ToC4M(),
        CompanyId = ResourceIdentifier.Create("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString()).ToC4M()
    };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static KBGroupPerson ToC4mPerson(this C4mUserInfoData userInfo, Identity humanUser)
        => new()
        {
            Id = ResourceIdentifier.Create("PM", "KBGroupPerson", humanUser, userInfo.PersonId.ToString()).ToC4M(),
            Surname = userInfo.PersonSurname,
            OrgUnit = new OrgUnit
            {
                Id = userInfo.PersonOrgUnitId.ToString(),
                JobPost = new JobPost
                {
                    Id = userInfo.PersonJobPostId
                },
                Name = userInfo.PersonOrgUnitName
            }
        };
}
