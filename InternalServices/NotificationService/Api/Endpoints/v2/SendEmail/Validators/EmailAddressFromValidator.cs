using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail.Validators;

internal sealed class EmailAddressFromValidator : AbstractValidator<EmailAddress>
{
    // todo: error code for emailAddress.Value
    private static readonly Func<string, string> _normalForm = e => e.ToLowerInvariant();
    public EmailAddressFromValidator(AppConfiguration appConfiguration)
    {
        var allowedDomainNames = appConfiguration.EmailSenders.Mcs
            .Union(appConfiguration.EmailSenders.Mpss)
            .Select(_normalForm)
            .ToHashSet();

        RuleFor(emailAddress => emailAddress)
            .SetValidator(new EmailAddressValidator());
        
        RuleFor(emailAddress => emailAddress.Value)
            .Must(email =>
            {
                var domainName = _normalForm(email).Split('@').Last();
                return allowedDomainNames.Contains(domainName);
            })
            .WithMessage($"Allowed domain names for sender: {string.Join(", ", allowedDomainNames)}.");
    }
}