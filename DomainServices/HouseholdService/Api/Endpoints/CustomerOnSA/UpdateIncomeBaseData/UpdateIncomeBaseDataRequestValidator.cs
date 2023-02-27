using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncomeBaseData;

internal sealed class UpdateIncomeBaseDataRequestValidator
    : AbstractValidator<UpdateIncomeBaseDataRequest>
{
    public UpdateIncomeBaseDataRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.IncomeId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.IncomeIdIsEmpty);

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.IncomeBaseDataValidator(codebookService));
            });
    }
}
