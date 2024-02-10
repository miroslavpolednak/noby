using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetOfferDetail;

internal sealed class GetOfferDetailRequestValidator 
    : AbstractValidator<GetOfferDetailRequest>
{
    public GetOfferDetailRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}