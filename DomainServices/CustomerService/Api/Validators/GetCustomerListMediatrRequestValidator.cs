using DomainServices.CustomerService.Api.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class GetCustomerListMediatrRequestValidator : AbstractValidator<GetCustomerListMediatrRequest>
{
    public GetCustomerListMediatrRequestValidator()
    {
        RuleFor(r => r.Identities).ForEach(item => item.SetValidator(new IdentityValidator()));
    }
}
