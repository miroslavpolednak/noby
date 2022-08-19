namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

internal static class AmountExtensions
{
    public static Amount? ToAmount(this decimal? amount)
        => amount.HasValue ? new Amount { CurrencyCode = "CZK", Value = amount } : null;

    public static Amount ToAmount(this decimal amount)
        => new Amount { CurrencyCode = "CZK", Value = amount };
}
