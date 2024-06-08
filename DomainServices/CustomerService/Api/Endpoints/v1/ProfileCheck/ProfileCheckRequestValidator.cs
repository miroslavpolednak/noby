using CIS.Infrastructure.CisMediatR.GrpcValidation;
using DomainServices.CustomerService.Api.Validators;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Endpoints.v1.ProfileCheck;

internal sealed class ProfileCheckRequestValidator
    : AbstractValidator<ProfileCheckRequest>
{
    public ProfileCheckRequestValidator()
    {
        RuleFor(r => r.Identity).SetValidator(new IdentityValidator());

        RuleFor(r => r.Identity.IdentityScheme)
            .Equal(SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)
            .WithErrorCode(ErrorCodeMapper.InvalidIdentityScheme);

        RuleFor(r => r.CustomerProfileCode)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.ProfileCodeIsEmpty);
    }
}