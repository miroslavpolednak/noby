using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.GetMortgageByOfferId;

internal sealed class GetMortgageByOfferIdRequestValidator
    : AbstractValidator<GetMortgageByOfferIdRequest>
{
    public GetMortgageByOfferIdRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId must be > 0");
    }
}