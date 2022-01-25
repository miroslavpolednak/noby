using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateCaseStateMediatrRequestValidator : AbstractValidator<Dto.UpdateCaseStateMediatrRequest>
{
    public UpdateCaseStateMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.State)
            .GreaterThan(0)
            .WithMessage("Case State must be > 0").WithErrorCode("13017");
    }
}
