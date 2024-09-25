using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.GetCustomerDetail;

internal sealed class GetCustomerDetailRequestValidator
    : AbstractValidator<GetCustomerDetailRequest>
{
    public GetCustomerDetailRequestValidator()
    {
        RuleFor(r => r.Identity)
            .SetValidator(new IdentityValidator());
    }
}
