using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class GetCustomerListMediatrRequestValidator : AbstractValidator<Dto.GetCustomerListMediatrRequest>
{
    public GetCustomerListMediatrRequestValidator()
    {
        RuleFor(t => t.Request.Identities)
            .Must(t => t != null && t.Any())
            .WithMessage("At least one of field is required").WithErrorCode("17000")
            .ForEach(item =>
            {
                item.SetValidator(new IdentityValidator());
            });
    }
}
