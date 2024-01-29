using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;

internal sealed class SendEmailsJobConfigurationValidator
    : AbstractValidator<SendEmailsJobConfiguration>
{
    public SendEmailsJobConfigurationValidator()
    {
        RuleFor(t => t.SmtpConfiguration.Host)
            .NotEmpty();

        RuleFor(t => t.SmtpConfiguration.Port)
            .NotEmpty();

        RuleFor(t => t.SmtpConfiguration.Timeout)
            .InclusiveBetween(10, 300);

        RuleFor(t => t.NumberOfEmailsAtOnce)
            .NotEmpty();

        RuleFor(t => t.EmailDomainWhitelist)
            .NotEmpty();
    }
}
