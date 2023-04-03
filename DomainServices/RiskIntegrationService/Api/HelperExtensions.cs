using CIS.Core.Types;

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

    public static long? ToSalesArrangementId(this string resourceIdentifier, string environmentName)
    {
        if (string.IsNullOrEmpty(resourceIdentifier))
            return null;

        var id = resourceIdentifier.Split(".").Last().Split("~")[0];
        id = id.Replace(environmentName, "", StringComparison.InvariantCultureIgnoreCase);

        return long.Parse(id, System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string? ToPrimaryCustomerId(this string resourceIdentifier)
        => !string.IsNullOrEmpty(resourceIdentifier) ? resourceIdentifier.Split(".").Last() : null;

    public static string ToEnvironmentId(this long salesArrangementId, string environmentName)
        => environmentName.ToLower(System.Globalization.CultureInfo.InvariantCulture) switch
        {
            "prod" => salesArrangementId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            _ => $"{environmentName}{salesArrangementId}"
        };
}