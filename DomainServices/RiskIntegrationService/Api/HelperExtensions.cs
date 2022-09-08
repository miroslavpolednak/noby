namespace DomainServices.RiskIntegrationService.Api;

internal static class HelperExtensions
{
    public static string? ConvertToString(this Contracts.Shared.BankAccountDetail? bankAccount)
        => bankAccount switch
        {
            { } acc when !string.IsNullOrEmpty(acc?.NumberPrefix) => $"{bankAccount.NumberPrefix}-{bankAccount.Number}/{bankAccount.BankCode}",
            { } acc when !string.IsNullOrEmpty(acc?.Number) => $"{bankAccount.Number}/{bankAccount.BankCode}",
            _ => null
        };

    public static long? ToSalesArrangementId(this string resourceIdentifier)
        => !string.IsNullOrEmpty(resourceIdentifier)
            ? long.Parse(resourceIdentifier.Split(".").Last().Split("~")[0], System.Globalization.CultureInfo.InvariantCulture)
            : null;

    public static string? ToPrimaryCustomerId(this string resourceIdentifier)
        => !string.IsNullOrEmpty(resourceIdentifier) ? resourceIdentifier.Split(".").Last() : null;
}