using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.ValidateContact;

internal sealed class ValidateContactRequestValidator : AbstractValidator<ValidateContactRequest>
{
    public ValidateContactRequestValidator()
    {
        RuleFor(request => request.ContactType)
            .NotEmpty()
                .WithMessage("ContactType required.")
                .WithErrorCode("0");

        RuleFor(request => request.Contact)
            .NotEmpty()
                .WithMessage("ContactType required.")
                .WithErrorCode("0");
    }
}