using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOffer;

internal sealed class GetMortgageOfferRequestValidator 
    : AbstractValidator<GetMortgageOfferRequest>
{
    public GetMortgageOfferRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}