using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateCaseStateMediatrRequestValidator : AbstractValidator<Dto.UpdateCaseStateMediatrRequest>
{
    public UpdateCaseStateMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13000");

        RuleFor(t => t.State)
            .GreaterThan(0)
            .WithMessage("State must be > 0").WithErrorCode("13000");
    }
}
