using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.Validators;

internal class CreateCaseRequestValidator
    : AbstractValidator<Dto.CreateCaseRequest>
{
    public CreateCaseRequestValidator()
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
