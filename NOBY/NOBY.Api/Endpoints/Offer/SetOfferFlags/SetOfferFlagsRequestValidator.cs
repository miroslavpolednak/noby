using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.SetOfferFlags;

internal sealed class SetOfferFlagsRequestValidator
    : AbstractValidator<SetOfferFlagsRequest>
{
    public SetOfferFlagsRequestValidator()
    {
        RuleFor(t => t.Flags)
            .NotEmpty();

        RuleForEach(t => t.Flags)
            .ChildRules(c =>
            {
                c.RuleFor(t => t.FlagType)
                    .IsInEnum();
            })
            .When(t => t.Flags is not null);
    }
}
