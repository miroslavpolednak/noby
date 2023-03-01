using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetOffer;

internal sealed class GetOfferRequestValidator 
    : AbstractValidator<GetOfferRequest>
{
    public GetOfferRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}