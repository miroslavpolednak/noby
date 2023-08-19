using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.RiskIntegrationService.ExternalServices;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2;

public static class Extensions
{
    public static _C4M.LoanApplicationDealer ToC4mDealer(this UserService.Contracts.UserRIPAttributes userInfo, Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
    => new()
    {
        Id = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser).ToC4M(),
        CompanyId = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString()).ToC4M()
    };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static _C4M.KBGroupPerson ToC4mPerson(this UserService.Contracts.UserRIPAttributes userInfo, Identity humanUser)
        => new()
        {
            Id = _C4M.ResourceIdentifier.Create("PM", "KBGroupPerson", humanUser, userInfo.PersonId.ToString()).ToC4M(),
            Surname = userInfo.PersonSurname,
            OrgUnit = new _C4M.OrgUnit()
            {
                Id = userInfo.PersonOrgUnitId.ToString(),
                JobPost = new _C4M.JobPost
                {
                    Id = userInfo.PersonJobPostId
                },
                Name = userInfo.PersonOrgUnitName
            }
        };

    public static _C4M.Amount? ToAmount(this decimal? amount)
        => amount.HasValue ? new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount.Value } : null;

    public static _C4M.Amount ToAmountDefault(this decimal? amount)
        => amount.HasValue ? amount.ToAmount()! : (0M).ToAmount();

    public static _C4M.Amount ToAmount(this decimal amount)
        => new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount };

    public static _C4M.Amount? ToAmount(this AmountDetail? amount)
        => amount is null ? null : new _C4M.Amount { CurrencyCode = amount.CurrencyCode ?? Constants.CurrencyCode, Value = amount.Amount };
}
