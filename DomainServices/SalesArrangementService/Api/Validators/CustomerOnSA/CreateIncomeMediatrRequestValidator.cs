using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateIncomeMediatrRequestValidator
    : AbstractValidator<Dto.CreateIncomeMediatrRequest>
{
    public CreateIncomeMediatrRequestValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("16024");

        RuleFor(t => t.Request.IncomeTypeId)
            .GreaterThan(0)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        RuleFor(t => t.Request.IncomeTypeId)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithMessage("IncomeTypeId must be > 0").WithErrorCode("16028");

        RuleFor(t => t.Request.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add<Contracts.IncomeBaseData>(new IncomeBaseDataValidator(codebookService));
            });

        RuleFor(t => t.Request.Employement)
            .Must(t => {
                if (t?.Employer is null)
                {
                    return true;
                }
                // nelze uvést Cin a BirthNumber zároveň
                return !(!String.IsNullOrEmpty(t.Employer.Cin) && !String.IsNullOrEmpty(t.Employer.BirthNumber));
            })
            .WithMessage("Only one of values can be set [Employement.Employer.Cin, Employement.Employer.BirthNumber]").WithErrorCode("16046");
    }
}
