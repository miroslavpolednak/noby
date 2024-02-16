using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Validators;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail.Validators;

internal sealed class EmailAddressFromValidator 
    : AbstractValidator<Contracts.v2.SendEmailRequest.Types.EmailAddress>
{
    private static HashSet<string>? _allowedDomainNames;

    public EmailAddressFromValidator(AppConfiguration appConfiguration)
    {
        // neni thread safe, to je jedno
        if (_allowedDomainNames is null)
        {
            _allowedDomainNames = appConfiguration.EmailSenders.Mcs
                .Union(appConfiguration.EmailSenders.Mpss)
                .ToHashSet();
        }

        RuleFor(emailAddress => emailAddress)
            .SetValidator(new EmailAddressValidator());

        RuleFor(emailAddress => emailAddress.Value)
            .Must(email => _allowedDomainNames.Contains(email.GetDomainFromEmail(), StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Allowed domain names for sender: {string.Join(", ", _allowedDomainNames)}.");
    }
}