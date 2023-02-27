using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncome;

internal sealed class UpdateIncomeRequestValidator
    : AbstractValidator<UpdateIncomeRequest>
{
    public UpdateIncomeRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.IncomeId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.IncomeIdIsEmpty);

        RuleFor(t => t.IncomeTypeId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.IncomeTypeIdIsEmpty)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithErrorCode(ValidationMessages.IncomeTypeIdIsEmpty);

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.IncomeBaseDataValidator(codebookService));
            });

        // nelze uvést Cin a BirthNumber zároveň
        RuleFor(t => t.Employement)
            .Must(t => !(!string.IsNullOrEmpty(t.Employer.Cin) && !string.IsNullOrEmpty(t.Employer.BirthNumber)))
            .WithErrorCode(ValidationMessages.EmployementCinBirthNo)
            .When(t => t.Employement?.Employer is not null);
    }
}