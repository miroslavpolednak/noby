namespace DomainServices.RiskIntegrationService.Api;

internal static class HelperExtensions
{
    public static string ConvertToString(this Contracts.Shared.BankAccountDetail? bankAccount)
        => bankAccount is null ? "" : string.IsNullOrEmpty(bankAccount.NumberPrefix) ? $"{bankAccount.Number}/{bankAccount.BankCode}" : $"{bankAccount.NumberPrefix}-{bankAccount.Number}/{bankAccount.BankCode}";
}