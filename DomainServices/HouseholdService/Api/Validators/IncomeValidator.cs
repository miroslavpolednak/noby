using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators;

internal sealed class IncomeValidator
    : AbstractValidator<IIncome>
{
    public IncomeValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.IncomeTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.IncomeTypeIdIsEmpty)
            .Must(t => (HouseholdTypes)t != HouseholdTypes.Unknown)
            .WithErrorCode(ErrorCodeMapper.IncomeTypeIdIsEmpty);

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new IncomeBaseDataValidator(codebookService));
            });

        // nelze uvést Cin a BirthNumber zároveň
        RuleFor(t => t.Employement)
            .Must(t => !(!string.IsNullOrEmpty(t.Employer.Cin) && !string.IsNullOrEmpty(t.Employer.BirthNumber)))
            .WithErrorCode(ErrorCodeMapper.EmployementCinBirthNo)
            .When(t => t.Employement?.Employer is not null);
    }
}