using DomainServices.HouseholdService.Api.Validators;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncome;

internal class UpdateIncomeMediatrRequestValidator
    : AbstractValidator<UpdateIncomeMediatrRequest>
{
    public UpdateIncomeMediatrRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.Request.IncomeId)
            .GreaterThan(0)
            .WithMessage("IncomeId must be > 0").WithErrorCode("16055");

        RuleFor(t => t.Request.IncomeTypeId)
            .GreaterThan(0)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        RuleFor(t => t.Request.IncomeTypeId)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        RuleFor(t => t.Request.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new IncomeBaseDataValidator(codebookService));
            });

        RuleFor(t => t.Request.Employement)
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