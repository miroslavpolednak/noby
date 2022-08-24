namespace DomainServices.RiskIntegrationService.Api;

internal static class HelperExtensions
{
    public static string ConvertToString(this Contracts.Shared.BankAccountDetail? bankAccount)
        => bankAccount is null ? "" : string.IsNullOrEmpty(bankAccount.NumberPrefix) ? $"{bankAccount.Number}/{bankAccount.BankCode}" : $"{bankAccount.NumberPrefix}-{bankAccount.Number}/{bankAccount.BankCode}";

    public static long? ToSalesArrangementId(this string resourceIdentifier)
    {
        if (string.IsNullOrEmpty(resourceIdentifier)) 
            return null;

        var s = resourceIdentifier.Split(".");
        return long.Parse(s.Last().Split("~")[0], System.Globalization.CultureInfo.InvariantCulture);
    }

    public static string? ToPrimaryCustomerId(this string resourceIdentifier)
        => !string.IsNullOrEmpty(resourceIdentifier) ? resourceIdentifier.Split(".").Last() : null;
}