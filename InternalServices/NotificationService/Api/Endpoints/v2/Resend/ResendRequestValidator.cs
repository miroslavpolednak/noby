using CIS.InternalServices.NotificationService.Contracts.v2;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.Resend;

internal sealed class ResendRequestValidator
    : AbstractValidator<ResendRequest>
{
    public ResendRequestValidator()
    {
        RuleFor(request => request.NotificationId)
            .NotEmpty()
            .Must(t => Guid.TryParse(t, out Guid _))
            .WithErrorCode(ErrorCodeMapper.NotificationIdRequired);
    }
}
