using DomainServices.CustomerService.Api.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class CreateCustomerMediatrRequestValidator : AbstractValidator<CreateCustomerMediatrRequest>
{
    public CreateCustomerMediatrRequestValidator()
    {
        RuleFor(m => m.Request.Identity.IdentityScheme)
            .IsInEnum()
            .NotEqual(Identity.Types.IdentitySchemes.Unknown)
            .WithMessage("IdentityScheme must be specified")
            .WithErrorCode("11006");

        When(m => m.Request.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Kb,
             () => RuleFor(m => m.Request.Identity.IdentityId)
                   .Empty()
                   .WithMessage("")
                   .WithErrorCode("11016"));

        When(m => m.Request.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Mp,
             () => RuleFor(m => m.Request.Identity.IdentityId)
                   .NotEmpty()
                   .GreaterThan(0)
                   .WithMessage("")
                   .WithErrorCode("11016"));
    }
}