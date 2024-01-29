using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SetExpiredEmails;

internal sealed class SetExpiredEmailsJobConfigurationValidator
    : AbstractValidator<SetExpiredEmailsJobConfiguration>
{
    public SetExpiredEmailsJobConfigurationValidator()
    {
        RuleFor(t => t.EmailSlaInMinutes)
            .NotEmpty();
    }
}
