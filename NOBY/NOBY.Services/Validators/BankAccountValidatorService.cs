using System.Text.RegularExpressions;
using DomainServices.CodebookService.Clients;
using SharedTypes.Interfaces;

namespace NOBY.Services.Validators;

[ScopedService, AsImplementedInterfacesService]
public partial class BankAccountValidatorService : IBankAccountValidatorService
{
    private readonly int[] _weights = [ 1, 2, 4, 8, 5, 10, 9, 7, 3, 6 ];

    private readonly ICodebookServiceClient _codebookService;

    public BankAccountValidatorService(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }

    public bool IsBankAccountValid(IBankAccount? bankAccount)
    {
        if (bankAccount is null)
        {
            return true;
        }

        return IsBankAccountValid(bankAccount.AccountPrefix, bankAccount.AccountNumber ?? string.Empty);
    }

    public bool IsBankAccountValid(string? prefix, string accountNumber)
    {
        var cleanAccountNumber = CleanNonNumberCharactersRegex().Replace(accountNumber, "");

        if (CalculateControlNumber(cleanAccountNumber) % 11 != 0)
            return false;

        if (string.IsNullOrWhiteSpace(prefix))
            return true;

        var cleanAccountPrefix = CleanNonNumberCharactersRegex().Replace(prefix, "");

        return CalculateControlNumber(cleanAccountPrefix) % 11 == 0;

        int CalculateControlNumber(string number) =>
            number.Reverse().Select((character, index) => int.Parse(character.ToString(), CultureInfo.InvariantCulture) * _weights.ElementAtOrDefault(index)).Sum();
    }

    public bool IsBankCodeValid(string? bankCode, CancellationToken cancellationToken = default)
    {
        if (bankCode is null)
        {
            return true;
        }

        var bankCodes = _codebookService.BankCodes(cancellationToken).GetAwaiter().GetResult();

        return bankCodes.Any(c => c.BankCode.Equals(bankCode, StringComparison.Ordinal));
    }

    public bool IsBankAccountAndCodeValid(IBankAccount? bankAccount, CancellationToken cancellationToken = default)
    {
        return IsBankAccountValid(bankAccount) && IsBankCodeValid(bankAccount?.AccountBankCode, cancellationToken);
    }

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex CleanNonNumberCharactersRegex();
}