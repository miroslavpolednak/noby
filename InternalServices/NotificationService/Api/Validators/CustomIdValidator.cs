using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

internal sealed class CustomIdValidator 
    : AbstractValidator<string>
{
    public CustomIdValidator()
    {
        RuleFor(customId => customId)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
            .WithErrorCode(ErrorCodeMapper.CustomIdInvalid);
    }
}