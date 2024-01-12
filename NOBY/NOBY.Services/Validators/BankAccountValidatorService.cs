using System.Text.RegularExpressions;
using DomainServices.CodebookService.Clients;
using NOBY.Dto;

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
        var accountNumberFull = CleanNonNumberCharactersRegex().Replace($"{prefix}{accountNumber}", "");

        var controlNumber = accountNumberFull.Reverse()
                                             .Select((character, index) => int.Parse(character.ToString(), CultureInfo.InvariantCulture) * _weights.ElementAtOrDefault(index))
                                             .Sum();

        return controlNumber % 11 == 0;
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

    public bool IsBankAccoungAndCodeValid(IBankAccount? bankAccount, CancellationToken cancellationToken = default)
    {
        return IsBankAccountValid(bankAccount) && IsBankCodeValid(bankAccount?.AccountBankCode, cancellationToken);
    }

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex CleanNonNumberCharactersRegex();
}