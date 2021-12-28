using FluentValidation;

namespace FOMS.Api.Endpoints.Savings.Offer.Validators;

internal class CreateDraftValidator
    : AbstractValidator<Dto.CreateDraftRequest>
{
    public CreateDraftValidator()
    {
        RuleFor(t => t.OfferInstanceId)
            .GreaterThan(0).WithMessage("Případ nebylo možné napárovat na simulaci");

        RuleFor(t => t.LastName)
            .Must((data, name) => data.Customer is not null || !string.IsNullOrEmpty(name))
            .WithMessage("Pro uložení modelace je potřeba zadat nacionále klienta");

        RuleFor(t => t.FirstName)
            .Must((data, name) => data.Customer is not null || !string.IsNullOrEmpty(name))
            .WithMessage("Pro uložení modelace je potřeba zadat nacionále klienta");
    }
}
