using FluentValidation;

namespace FOMS.Api.Endpoints.Savings.Offer.Validators;

internal class CreateCaseValidator
    : AbstractValidator<Dto.CreateCaseRequest>
{
    public CreateCaseValidator()
    {
        RuleFor(t => t.OfferInstanceId)
            .GreaterThan(0).WithMessage("Případ nebylo možné napárovat na simulaci");

        RuleFor(t => t.CreateProduct)
            .Must((data, createProduct) => !createProduct || data.Customer is not null)
            .WithMessage("Klient nebyl zadán");

        RuleFor(t => t.CreateProduct)
            .Must((data, createProduct) => createProduct || data.Customer is not null || (!string.IsNullOrEmpty(data.Request.FirstName) && !string.IsNullOrEmpty(data.Request.LastName)))
            .WithMessage("Pro uložení modelace je potřeba zadat nacionále klienta");
    }
}
