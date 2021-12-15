using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class LinkOwnerToCaseMediatrRequestValidator : AbstractValidator<Dto.LinkOwnerToCaseMediatrRequest>
{
    public LinkOwnerToCaseMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13000");

        RuleFor(t => t.PartyId)
            .GreaterThan(0)
            .WithMessage("PartyId must be > 0").WithErrorCode("13000");
    }
}
