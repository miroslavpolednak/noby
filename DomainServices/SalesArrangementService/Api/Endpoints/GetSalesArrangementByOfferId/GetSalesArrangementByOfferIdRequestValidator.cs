using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangementByOfferId;

internal sealed class GetSalesArrangementByOfferIdRequestValidator
    : AbstractValidator<Contracts.GetSalesArrangementByOfferIdRequest>
{
    public GetSalesArrangementByOfferIdRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId must be > 0").WithErrorCode("18011");
    }
}