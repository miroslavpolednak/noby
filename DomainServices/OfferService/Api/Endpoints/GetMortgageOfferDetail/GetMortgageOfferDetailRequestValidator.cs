using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferDetail;

internal sealed class GetMortgageOfferDetailRequestValidator 
    : AbstractValidator<GetMortgageOfferDetailRequest>
{
    public GetMortgageOfferDetailRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}