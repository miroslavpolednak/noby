using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Common;

public class CustomIdValidator : AbstractValidator<string>
{
    public CustomIdValidator()
    {
        RuleFor(customId => customId)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.CustomIdInvalid);
    }
}