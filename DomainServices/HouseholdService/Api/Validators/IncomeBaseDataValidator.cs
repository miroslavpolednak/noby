using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators;

public class IncomeBaseDataValidator
    : AbstractValidator<IncomeBaseData>
{
    public IncomeBaseDataValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        // check codebooks
        RuleFor(t => t.CurrencyCode)
            .MustAsync(async (currencyCode, cancellation) =>
            {
                return (await codebookService.Currencies(cancellation)).Any(t => t.Code == currencyCode);
            })
            .When(t => !string.IsNullOrEmpty(t.CurrencyCode))
            .WithMessage("CurrencyId is not valid").WithErrorCode("16030");
    }
}
