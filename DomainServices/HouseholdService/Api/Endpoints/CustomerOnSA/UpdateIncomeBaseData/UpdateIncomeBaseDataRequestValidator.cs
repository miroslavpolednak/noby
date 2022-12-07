using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncomeBaseData;

internal class UpdateIncomeBaseDataRequestValidator
    : AbstractValidator<UpdateIncomeBaseDataRequest>
{
    public UpdateIncomeBaseDataRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.IncomeId)
            .GreaterThan(0)
            .WithMessage("IncomeId must be > 0").WithErrorCode("16055");

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.IncomeBaseDataValidator(codebookService));
            });
    }
}
