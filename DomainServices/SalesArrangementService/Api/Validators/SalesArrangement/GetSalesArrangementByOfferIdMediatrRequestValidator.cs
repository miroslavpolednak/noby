using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal sealed class GetSalesArrangementByOfferIdMediatrRequestValidator
    : AbstractValidator<Dto.GetSalesArrangementByOfferIdMediatrRequest>
{
    public GetSalesArrangementByOfferIdMediatrRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId must be > 0").WithErrorCode("16008");
    }
}