namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;

public static class BankAccountHelper
{
    public static string AccountNumber(string? prefix, string accountNumber, string bankCode)
    {
        return string.IsNullOrWhiteSpace(prefix) ? $"{accountNumber}/{bankCode}" : $"{prefix}-{accountNumber}/{bankCode}";
    }
}