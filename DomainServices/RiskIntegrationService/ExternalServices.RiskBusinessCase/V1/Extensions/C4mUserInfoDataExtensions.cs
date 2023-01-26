using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.RiskIntegrationService.ExternalServices.Dto;

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V1.Contracts;

public static class C4mUserInfoDataExtensions
{
    public static LoanApplicationDealer ToC4mDealer(this C4mUserInfoData userInfo, Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
    => new()
    {
        Id = ResourceIdentifier.CreateUser("BM", "Broker", humanUser),
        CompanyId = ResourceIdentifier.CreateUser("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString())
    };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static KBGroupPerson ToC4mPerson(this C4mUserInfoData userInfo, Identity humanUser)
        => new()
        {
            Id = ResourceIdentifier.CreateUser("PM", "KBGroupPerson", humanUser),
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
