using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class LinkOwnerToCaseMediatrRequestValidator : AbstractValidator<Dto.LinkOwnerToCaseMediatrRequest>
{
    public LinkOwnerToCaseMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithMessage("CaseOwnerUserId must be > 0").WithErrorCode("13003");
    }
}
