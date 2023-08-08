using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2.Contracts;
using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.RiskIntegrationService.ExternalServices;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2;

public static class RiskCharacteristicsExtensions
{
    public static _C4M.Amount? ToRiskCharacteristicsAmount(this decimal? amount)
        => amount.HasValue ? new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount.Value } : null;

    //public static _C4M.Amount ToAmountDefault(this decimal? amount)
    //    => amount.HasValue ? amount.ToAmount()! : (0M).ToAmount();

    public static _C4M.Amount ToRiskCharacteristicsAmount(this decimal amount)
        => new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount };

    public static _C4M.Amount ToRiskCharacteristicsAmount(this int amount)
        => new _C4M.Amount { CurrencyCode = Constants.CurrencyCode, Value = amount };

    public static _C4M.Amount? ToRiskCharacteristicsAmount(this DomainServices.RiskIntegrationService.Contracts.Shared.AmountDetail? amount)
        => amount is null ? null : new _C4M.Amount { CurrencyCode = amount.CurrencyCode ?? Constants.CurrencyCode, Value = amount.Amount };
}
