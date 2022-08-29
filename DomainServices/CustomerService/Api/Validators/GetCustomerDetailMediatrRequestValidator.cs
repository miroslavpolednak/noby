using DomainServices.CustomerService.Dto;
using FluentValidation;
namespace DomainServices.CustomerService.Api.Validators;

internal class GetCustomerDetailMediatrRequestValidator : AbstractValidator<GetCustomerDetailMediatrRequest>
{
    public GetCustomerDetailMediatrRequestValidator()
    {
        RuleFor(t => t.Request.Identity)
            .NotNull()
            .WithMessage("IdentityId must be not empty").WithErrorCode("17000")
            .SetValidator(new IdentityValidator());
    }
}
