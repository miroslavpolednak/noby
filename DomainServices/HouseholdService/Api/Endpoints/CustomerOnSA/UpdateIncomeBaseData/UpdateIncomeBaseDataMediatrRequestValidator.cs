using DomainServices.HouseholdService.Api.Validators;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncomeBaseData;

internal class UpdateIncomeBaseDataMediatrRequestValidator
    : AbstractValidator<UpdateIncomeBaseDataMediatrRequest>
{
    public UpdateIncomeBaseDataMediatrRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.Request.IncomeId)
            .GreaterThan(0)
            .WithMessage("IncomeId must be > 0").WithErrorCode("16055");

        RuleFor(t => t.Request.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new IncomeBaseDataValidator(codebookService));
            });
    }
}
