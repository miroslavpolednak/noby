using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.ValidateContact;

internal sealed class ValidateContactRequestValidator : AbstractValidator<ValidateContactRequest>
{
    public ValidateContactRequestValidator()
    {
        RuleFor(request => request.Contact)
            .NotEmpty()
                .WithMessage("Contact must not be empty.")
                .WithErrorCode("11031");
        
        RuleFor(request => request.ContactType)
            .NotEmpty()
                .WithMessage("ContactType must not be empty.")
                .WithErrorCode("11032");
    }
}