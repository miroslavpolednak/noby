using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.GetCustomerList;

internal sealed class GetCustomerListRequestValidator
    : AbstractValidator<CustomerListRequest>
{
    public GetCustomerListRequestValidator()
    {
        RuleFor(r => r.Identities)
            .ForEach(item => item.SetValidator(new IdentityValidator()));
    }
}
