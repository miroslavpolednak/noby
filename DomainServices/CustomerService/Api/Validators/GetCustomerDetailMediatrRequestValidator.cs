using FluentValidation;
namespace DomainServices.CustomerService.Api.Validators;

internal class GetCustomerDetailMediatrRequestValidator : AbstractValidator<Dto.GetCustomerDetailMediatrRequest>
{
    public GetCustomerDetailMediatrRequestValidator()
    {
        RuleFor(t => t.Request.Identity)
            .NotNull()
            .WithMessage("IdentityId must be not empty").WithErrorCode("17000")
            .SetValidator(new IdentityValidator());
            
    }
}
