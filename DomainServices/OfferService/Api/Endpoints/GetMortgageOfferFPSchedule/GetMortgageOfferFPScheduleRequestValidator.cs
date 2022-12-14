using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferFPSchedule;

internal class GetMortgageOfferFPScheduleRequestValidator 
    : AbstractValidator<GetMortgageOfferFPScheduleRequest>
{
    public GetMortgageOfferFPScheduleRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not specified").WithErrorCode("10001");
    }
}