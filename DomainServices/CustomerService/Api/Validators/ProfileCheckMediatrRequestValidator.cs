using DomainServices.CustomerService.Api.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class ProfileCheckMediatrRequestValidator : AbstractValidator<ProfileCheckMediatrRequest>
{
    public ProfileCheckMediatrRequestValidator()
    {
        RuleFor(r => r.Request.Identity)
            .NotNull()
            .WithMessage("")
            .WithErrorCode("99999") //TODO: ErrorCode
            .SetValidator(new IdentityValidator());
    }
}