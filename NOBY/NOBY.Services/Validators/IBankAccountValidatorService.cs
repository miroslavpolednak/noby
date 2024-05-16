using NOBY.Dto;

namespace NOBY.Services.Validators;

public interface IBankAccountValidatorService
{
    bool IsBankAccountValid(IBankAccount? bankAccount);
    bool IsBankAccountValid(string? prefix, string accountNumber);
    bool IsBankCodeValid(string bankCode, CancellationToken cancellationToken = default);
    bool IsBankAccountAndCodeValid(IBankAccount? bankAccount, CancellationToken cancellationToken = default);
}