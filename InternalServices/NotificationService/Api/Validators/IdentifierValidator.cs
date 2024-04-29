using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

internal sealed class IdentifierValidator 
    : AbstractValidator<SharedTypes.GrpcTypes.UserIdentity>
{
    public IdentifierValidator()
    {
        RuleFor(request => request.Identity)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.IdentityRequired)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorCodeMapper.IdentityInvalid);

        RuleFor(request => request.IdentityScheme)
            .Must(t => t != SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.Unknown && Enum.IsDefined(t))
            .WithErrorCode(ErrorCodeMapper.IdentitySchemeInvalid);
    }
}