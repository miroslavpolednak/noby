using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncome;

internal class UpdateIncomeRequestValidator
    : AbstractValidator<UpdateIncomeRequest>
{
    public UpdateIncomeRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.IncomeId)
            .GreaterThan(0)
            .WithMessage("IncomeId must be > 0").WithErrorCode("16055");

        RuleFor(t => t.IncomeTypeId)
            .GreaterThan(0)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        RuleFor(t => t.IncomeTypeId)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.IncomeBaseDataValidator(codebookService));
            });

        RuleFor(t => t.Employement)
            .Must(t =>
            {
                if (t?.Employer is null)
                {
                    return true;
                }
                // nelze uvést Cin a BirthNumber zároveň
                return !(!string.IsNullOrEmpty(t.Employer.Cin) && !string.IsNullOrEmpty(t.Employer.BirthNumber));
            })
            .WithMessage("Only one of values can be set [Employement.Employer.Cin, Employement.Employer.BirthNumber]").WithErrorCode("16046");
    }
}