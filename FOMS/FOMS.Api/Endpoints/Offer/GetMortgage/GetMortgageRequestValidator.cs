using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.GetMortgage;

internal class GetMortgageRequestValidator
    : AbstractValidator<Dto.GetMortgageRequest>
{
    public GetMortgageRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is empty");
    }
}