using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators;

internal sealed class IncomeValidator
    : AbstractValidator<IIncome>
{
    public IncomeValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.IncomeTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.IncomeTypeIdIsEmpty)
            .Must(t => (HouseholdTypes)t != HouseholdTypes.Unknown)
            .WithErrorCode(ErrorCodeMapper.IncomeTypeIdIsEmpty);

        RuleFor(t => t.BaseData)
            .ChildRules(baseData =>
            {
                baseData.RuleFor(t => t.CurrencyCode)
                    .MustAsync(async (currencyCode, cancellation) =>
                    {
                        return (await codebookService.Currencies(cancellation)).Any(t => t.Code == currencyCode);
                    })
                    .When(t => !string.IsNullOrEmpty(t.CurrencyCode))
                    .WithErrorCode(ErrorCodeMapper.CurrencyNotValid);
            });

        // nelze uvést Cin a BirthNumber zároveň
        RuleFor(t => t.Employement)
            .Must(t => !(!string.IsNullOrEmpty(t.Employer.Cin) && !string.IsNullOrEmpty(t.Employer.BirthNumber)))
            .WithErrorCode(ErrorCodeMapper.EmployementCinBirthNo)
            .When(t => t.Employement?.Employer is not null);
    }
}
