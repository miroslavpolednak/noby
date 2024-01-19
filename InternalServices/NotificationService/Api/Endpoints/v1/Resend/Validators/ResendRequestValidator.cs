using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Resend;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Resend.Validators;

public class ResendRequestValidator 
    : AbstractValidator<ResendRequest>
{
    public ResendRequestValidator()
    {
        RuleFor(request => request.NotificationId)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.NotificationIdRequired);
    }
}
