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
            .NotEqual(ContactType.Unknown)
                .WithMessage("ContactType must not be empty.")
                .WithErrorCode("11032")
            .IsInEnum()
                .WithMessage("ContactType has unexpected value.")
                .WithErrorCode("11033");
    }
}