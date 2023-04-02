using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.GetFullPaymentScheduleByOfferId;

internal sealed class GetFullPaymentScheduleRequestValidator
    : AbstractValidator<GetFullPaymentScheduleByOfferIdRequest>
{
    public GetFullPaymentScheduleRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId must be > 0");
    }
}