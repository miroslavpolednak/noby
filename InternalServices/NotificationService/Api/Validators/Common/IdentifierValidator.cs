using CIS.InternalServices.NotificationService.Contracts.Common;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Common;

public class IdentifierValidator : AbstractValidator<Identifier>
{
    public IdentifierValidator()
    {
        RuleFor(request => request.Identity)
            .NotEmpty()
                .WithMessage($"{nameof(Identifier.Identity)} required.");

        RuleFor(request => request.IdentityScheme)
            .NotEmpty()
                .WithMessage($"{nameof(Identifier.IdentityScheme)} required.");
    }
}