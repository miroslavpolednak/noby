using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Common;

public class IdentifierValidator : AbstractValidator<Identifier>
{
    public IdentifierValidator()
    {
        RuleFor(request => request.Identity)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.IdentityRequired)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorCodeMapper.IdentityInvalid);

        RuleFor(request => request.IdentityScheme)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.IdentitySchemeRequired)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorCodeMapper.IdentitySchemeInvalid);
    }
}