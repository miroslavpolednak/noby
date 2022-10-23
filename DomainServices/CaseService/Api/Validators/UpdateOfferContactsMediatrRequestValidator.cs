using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal sealed class UpdateOfferContactsMediatrRequestValidator : AbstractValidator<Dto.UpdateOfferContactsMediatrRequest>
{
    public UpdateOfferContactsMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");
    }
}
