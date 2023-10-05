using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;
using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.RiskIntegrationService.ExternalServices;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2;

public static class CreditWorthinessExtensions
{
    public static _C4M.LoanApplicationDealer ToC4mDealer(this UserService.Contracts.UserRIPAttributes userInfo, Identity humanUser)
#pragma warning disable CA1305 // Specify IFormatProvider
    => new()
    {
        Id = _C4M.ResourceIdentifier.CreateResourceIdentifier("BM", "Broker", humanUser).ToC4M(),
        CompanyId = _C4M.ResourceIdentifier.CreateResourceIdentifier("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString()).ToC4M()
    };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static _C4M.KbGroupPerson ToC4mPerson(this UserService.Contracts.UserRIPAttributes userInfo, Identity humanUser)
        => new()
        {
            Id = _C4M.ResourceIdentifier.CreateResourceIdentifier("PM", "KBGroupPerson", humanUser, userInfo.PersonId.ToString()).ToC4M(),
            Surname = userInfo.PersonSurname,
            OrgUnit = new _C4M.OrgUnit()
            {
                Id = userInfo.PersonOrgUnitId,
                JobPost = new _C4M.JobPost
                {
                    Id = userInfo.PersonJobPostId
                },
                Name = userInfo.PersonOrgUnitName
            }
        };

    public static _C4M.Amount? ToCreditWorthinessAmount(this int? amount)
        => amount.HasValue ? amount.ToCreditWorthinessAmount() : null;

    public static _C4M.Amount? ToCreditWorthinessAmount(this int amount)
        => new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount };

    public static _C4M.Amount? ToCreditWorthinessAmount(this decimal? amount)
        => amount.HasValue ? new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount.Value } : null;

    public static _C4M.Amount ToCreditWorthinessAmountDefault(this decimal? amount)
        => amount.HasValue ? amount.ToCreditWorthinessAmount()! : (0M).ToCreditWorthinessAmount();

    public static _C4M.Amount ToCreditWorthinessAmount(this decimal amount)
        => new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount };

    public static _C4M.Amount? ToCreditWorthinessAmount(this AmountDetail? amount)
        => amount is null ? null : new _C4M.Amount { CurrencyCode = amount.CurrencyCode ?? Constants.CurrencyCode, Value = amount.Amount };
}
