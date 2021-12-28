using FluentValidation;

namespace FOMS.Api.Endpoints.Savings.Offer.Validators;

internal class CreateCaseValidator
    : AbstractValidator<Dto.CreateCaseRequest>
{
    public CreateCaseValidator()
    {
        RuleFor(t => t.OfferInstanceId)
            .GreaterThan(0).WithMessage("Případ nebylo možné napárovat na simulaci");

        RuleFor(t => t.Customer)
            .NotNull()
            .WithMessage("Klient nebyl zadán");
    }
}
