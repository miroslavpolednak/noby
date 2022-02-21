using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

internal class CreateMortgageCaseRequestValidator
    : AbstractValidator<CreateMortgageCaseRequest>
{
    public CreateMortgageCaseRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not set");

        RuleFor(t => t.FirstName)
            .NotEmpty()
            .When(t => t.Customer is null)
            .WithMessage("Jméno není vyplněné");

        RuleFor(t => t.LastName)
            .NotEmpty()
            .When(t => t.Customer is null)
            .WithMessage("Příjmení není vyplněné");
    }
}
