using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.ValidateContact;

internal sealed class ValidateContactRequestValidator : AbstractValidator<ValidateContactRequest>
{
    public ValidateContactRequestValidator()
    {
        RuleFor(request => request.ContactType)
            .NotEmpty();

        RuleFor(request => request.Contact)
            .NotEmpty();
    }
}