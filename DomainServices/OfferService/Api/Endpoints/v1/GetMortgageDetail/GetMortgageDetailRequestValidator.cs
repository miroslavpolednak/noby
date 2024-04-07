using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetMortgageDetail;

internal sealed class GetMortgageDetailRequestValidator
    : AbstractValidator<GetMortgageDetailRequest>
{
    public GetMortgageDetailRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}