
namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

public static class AmountExtensions
{
    public static Amount? ToAmount(this int? amount)
        => amount.HasValue ? amount.ToAmount() : null;

    public static Amount? ToAmount(this int amount)
        => new Amount { CurrencyCode = Constants.CurrencyCode, Value = amount };

    public static Amount? ToAmount(this decimal? amount)
        => amount.HasValue ? new Amount { CurrencyCode = Constants.CurrencyCode, Value = amount.Value } : null;

    public static Amount ToAmountDefault(this decimal? amount)
        => amount.HasValue ? amount.ToAmount()! : (0M).ToAmount();

    public static Amount ToAmount(this decimal amount)
        => new Amount { CurrencyCode = Constants.CurrencyCode, Value = amount };

    public static Amount? ToAmount(this DomainServices.RiskIntegrationService.Contracts.Shared.AmountDetail? amount)
        => amount is null ? null : new Amount { CurrencyCode = amount.CurrencyCode ?? Constants.CurrencyCode, Value = amount.Amount };
}
