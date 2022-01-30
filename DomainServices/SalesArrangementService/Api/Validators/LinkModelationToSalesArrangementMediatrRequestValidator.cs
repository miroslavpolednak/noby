using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class LinkModelationToSalesArrangementMediatrRequestValidator : AbstractValidator<Dto.LinkModelationToSalesArrangementMediatrRequest>
{
    public LinkModelationToSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("Sales arrangement ID does not exist.").WithErrorCode("16000");

        RuleFor(t => t.OfferInstanceId)
            .GreaterThan(0)
            .WithMessage("OfferInstance ID does not exist.").WithErrorCode("16001");
    }
}
