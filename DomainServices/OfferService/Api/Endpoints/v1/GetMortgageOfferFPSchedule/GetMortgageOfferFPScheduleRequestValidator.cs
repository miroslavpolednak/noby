using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetMortgageOfferFPSchedule;

internal sealed class GetMortgageOfferFPScheduleRequestValidator
    : AbstractValidator<GetMortgageOfferFPScheduleRequest>
{
    public GetMortgageOfferFPScheduleRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}