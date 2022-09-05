using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Api.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class ProfileCheckMediatrRequestValidator : AbstractValidator<ProfileCheckMediatrRequest>
{
    public ProfileCheckMediatrRequestValidator()
    {
        RuleFor(r => r.Request.Identity).SetValidator(new IdentityValidator());

        RuleFor(r => r.Request.Identity.IdentityScheme)
            .NotEqual(Identity.Types.IdentitySchemes.Kb)
            .WithMessage("Invalid identity scheme.")
            .WithErrorCode("11004");

        RuleFor(r => r.Request.CustomerProfileCode)
            .NotEmpty()
            .WithMessage("CustomerProfileCode must be specified")
            .WithErrorCode("11007");
    }
}