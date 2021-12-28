using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class LinkModelationToSalesArrangementMediatrRequestValidator : AbstractValidator<Dto.LinkModelationToSalesArrangementMediatrRequest>
{
    public LinkModelationToSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");

        RuleFor(t => t.OfferInstanceId)
            .GreaterThan(0)
            .WithMessage("OfferInstanceId must be > 0").WithErrorCode("13000");
    }
}
