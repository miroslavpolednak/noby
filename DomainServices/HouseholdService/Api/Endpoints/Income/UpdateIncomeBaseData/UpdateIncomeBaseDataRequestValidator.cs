using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.UpdateIncomeBaseData;

internal sealed class UpdateIncomeBaseDataRequestValidator
    : AbstractValidator<UpdateIncomeBaseDataRequest>
{
    public UpdateIncomeBaseDataRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.IncomeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.IncomeIdIsEmpty);

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.IncomeBaseDataValidator(codebookService));
            });
    }
}