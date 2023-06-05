using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Common;

public class IdentifierValidator : AbstractValidator<Identifier>
{
    public IdentifierValidator()
    {
        RuleFor(request => request.Identity)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentityRequired)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentityInvalid);

        RuleFor(request => request.IdentityScheme)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentitySchemeRequired)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentitySchemeInvalid);
    }
}