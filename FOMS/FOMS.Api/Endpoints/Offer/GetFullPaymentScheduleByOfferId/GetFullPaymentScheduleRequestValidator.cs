using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal class GetFullPaymentScheduleRequestValidator
    : AbstractValidator<GetFullPaymentScheduleByOfferIdRequest>
{
    public GetFullPaymentScheduleRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId must be > 0");
    }
}