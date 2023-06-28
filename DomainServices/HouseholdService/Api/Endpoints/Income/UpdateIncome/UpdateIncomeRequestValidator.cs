using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.UpdateIncome;

internal sealed class UpdateIncomeRequestValidator
    : AbstractValidator<UpdateIncomeRequest>
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