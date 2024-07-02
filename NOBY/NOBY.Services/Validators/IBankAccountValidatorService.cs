namespace NOBY.Services.Validators;

public interface IBankAccountValidatorService
{
    bool IsBankAccountValid(SharedTypes.Interfaces.IBankAccount? bankAccount);
    bool IsBankAccountValid(string? prefix, string accountNumber);
    bool IsBankCodeValid(string bankCode, CancellationToken cancellationToken = default);
    bool IsBankAccountAndCodeValid(SharedTypes.Interfaces.IBankAccount? bankAccount, CancellationToken cancellationToken = default);
}