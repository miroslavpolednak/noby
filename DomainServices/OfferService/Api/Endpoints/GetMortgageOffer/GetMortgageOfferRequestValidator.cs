using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOffer;

internal class GetMortgageOfferRequestValidator 
    : AbstractValidator<GetMortgageOfferRequest>
{
    public GetMortgageOfferRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not specified").WithErrorCode("10001");
    }
}