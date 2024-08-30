using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.UpdateIncome;

internal sealed class UpdateIncomeRequestValidator
    : AbstractValidator<Income>
{
    public UpdateIncomeRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.IncomeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.IncomeIdIsEmpty);

        RuleFor(t => t)
            .SetValidator(new Validators.IncomeValidator(codebookService));
    }
}