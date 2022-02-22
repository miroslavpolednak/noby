using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.GetMortgageByOfferId;

internal class GetMortgageByOfferIdRequestValidator
    : AbstractValidator<GetMortgageByOfferIdRequest>
{
    public GetMortgageByOfferIdRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId must be > 0");
    }
}