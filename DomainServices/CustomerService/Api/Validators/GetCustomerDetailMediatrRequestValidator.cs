using DomainServices.CustomerService.Api.Dto;
using FluentValidation;
namespace DomainServices.CustomerService.Api.Validators;

internal class GetCustomerDetailMediatrRequestValidator : AbstractValidator<GetCustomerDetailMediatrRequest>
{
    public GetCustomerDetailMediatrRequestValidator()
    {
        RuleFor(r => r.Identity).SetValidator(new IdentityValidator());
    }
}
