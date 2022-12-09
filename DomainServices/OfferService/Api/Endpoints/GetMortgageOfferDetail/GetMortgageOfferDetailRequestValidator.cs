using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferDetail;

internal class GetMortgageOfferDetailRequestValidator 
    : AbstractValidator<GetMortgageOfferDetailRequest>
{
    public GetMortgageOfferDetailRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not specified").WithErrorCode("10001");
    }
}