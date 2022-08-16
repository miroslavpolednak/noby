using FluentValidation;

namespace DomainServices.OfferService.Api.Validators;

internal class GetMortgageOfferFPScheduleRequestValidator : AbstractValidator<Dto.GetMortgageOfferFPScheduleMediatrRequest>
{
    public GetMortgageOfferFPScheduleRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not specified").WithErrorCode("10001");
    }
}