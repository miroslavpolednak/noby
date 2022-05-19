using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal sealed class DeleteCaseMediatrRequestValidator
    : AbstractValidator<Dto.DeleteCaseMediatrRequest>
{
    public DeleteCaseMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");
    }
}
