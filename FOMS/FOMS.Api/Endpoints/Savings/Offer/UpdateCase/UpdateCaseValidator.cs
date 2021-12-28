using FluentValidation;

namespace FOMS.Api.Endpoints.Savings.Offer.Validators;

internal class UpdateCaseValidator
    : AbstractValidator<Dto.UpdateCaseRequest>
{
    public UpdateCaseValidator()
    {
        RuleFor(t => t.OfferInstanceId)
            .GreaterThan(0).WithMessage("Případ nebylo možné napárovat na simulaci: OfferInstanceId");

        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0).WithMessage("Případ nebylo možné napárovat na simulaci: SalesArrangementId");
    }
}
