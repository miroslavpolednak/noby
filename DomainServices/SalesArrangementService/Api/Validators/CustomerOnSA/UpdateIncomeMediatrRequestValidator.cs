using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateIncomeMediatrRequestValidator
    : AbstractValidator<Dto.UpdateIncomeMediatrRequest>
{
    public UpdateIncomeMediatrRequestValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.IncomeId)
            .GreaterThan(0)
            .WithMessage("IncomeId must be > 0").WithErrorCode("16029");

        RuleFor(t => t.Request.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add<Contracts.IncomeBaseData>(new IncomeBaseDataValidator(codebookService));
            });
    }
}