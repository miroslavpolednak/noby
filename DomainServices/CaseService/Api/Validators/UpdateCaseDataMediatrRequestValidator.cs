using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateCaseDataMediatrRequestValidator 
    : AbstractValidator<Dto.UpdateCaseDataMediatrRequest>
{
    public UpdateCaseDataMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Request.Data.ProductTypeId)
            .GreaterThan(0)
            .WithMessage(t => "ProductTypeId must be > 0").WithErrorCode("13002");

        RuleFor(t => (decimal)t.Request.Data.TargetAmount)
            .GreaterThan(0)
            .WithMessage("Target amount must be > 0").WithErrorCode("13018");
    }
}
