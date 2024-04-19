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
                    .Cascade(CascadeMode.Stop)
                    .IsInEnum()
                    // validace na moznost zmeny
                    .Must(t => t is OfferFlagTypes.Liked or OfferFlagTypes.Selected);
            })
            .When(t => t.Flags is not null);

        
    }
}
