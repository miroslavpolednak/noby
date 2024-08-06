using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.UpdateContacts;

public class UpdateContactsRequestValidator : AbstractValidator<UpdateContactsRequest>
{
    public UpdateContactsRequestValidator()
    {
        RuleFor(r => r.Identity).SetValidator(new IdentityValidator());
    }
}