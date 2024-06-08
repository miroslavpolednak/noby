using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.GetCustomerDetail;

internal sealed class GetCustomerDetailRequestValidator
    : AbstractValidator<CustomerDetailRequest>
{
    public GetCustomerDetailRequestValidator()
    {
        RuleFor(r => r.Identity)
            .SetValidator(new IdentityValidator());
    }
}
