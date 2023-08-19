using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3.Contracts;
using DomainServices.RiskIntegrationService.Contracts.Shared;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2;

public static class Extension
{
    public static _C4M.LoanApplicationDealer ToC4mDealer(this UserService.Contracts.UserRIPAttributes userInfo, Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
    => new()
    {
        Id = _C4M.ResourceIdentifier.CreateUser("BM", "Broker", humanUser).ToC4M(),
        CompanyId = _C4M.ResourceIdentifier.CreateUser("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString()).ToC4M()
    };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static _C4M.KBGroupPerson ToC4mPerson(this UserService.Contracts.UserRIPAttributes userInfo, Identity humanUser)
        => new()
        {
            Id = _C4M.ResourceIdentifier.CreateUser("PM", "KBGroupPerson", humanUser, userInfo.PersonId.ToString()).ToC4M(),
            Surname = userInfo.PersonSurname,
            OrgUnit = new _C4M.OrgUnit
            {
                Id = userInfo.PersonOrgUnitId.ToString(),
                JobPost = new _C4M.JobPost
                {
                    Id = userInfo.PersonJobPostId
                },
                Name = userInfo.PersonOrgUnitName
            }
        };
}
