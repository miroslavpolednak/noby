using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators;

internal sealed class IncomeBaseDataValidator
    : AbstractValidator<IncomeBaseData>
{
    public IncomeBaseDataValidator(ICodebookServiceClient codebookService)
    {
        // check codebooks
        RuleFor(t => t.CurrencyCode)
            .MustAsync(async (currencyCode, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == currencyCode);
            })
            .When(t => !string.IsNullOrEmpty(t.CurrencyCode))
            .WithErrorCode(ErrorCodeMapper.CurrencyNotValid);
    }
}