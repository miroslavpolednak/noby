using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.GetCustomerList;

internal sealed class GetCustomerListRequestValidator : AbstractValidator<CustomerListRequest>
{
    public GetCustomerListRequestValidator()
    {
        RuleFor(r => r.Identities).ForEach(item => item.SetValidator(new IdentityValidator()));
    }
}
