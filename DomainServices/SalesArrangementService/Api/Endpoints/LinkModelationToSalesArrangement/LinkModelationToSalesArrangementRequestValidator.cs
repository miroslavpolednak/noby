using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.LinkModelationToSalesArrangement;

internal sealed class LinkModelationToSalesArrangementRequestValidator
    : AbstractValidator<Contracts.LinkModelationToSalesArrangementRequest>
{
    public LinkModelationToSalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("18010");

        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId Id must be > 0").WithErrorCode("18011");
    }
}
