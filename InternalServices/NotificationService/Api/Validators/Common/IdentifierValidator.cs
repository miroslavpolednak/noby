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
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentityRequired);

        RuleFor(request => request.IdentityScheme)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentitySchemeRequired);
    }
}