using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailAddressFromValidator : AbstractValidator<EmailAddress>
{
    // todo: error code for emailAddress.Value
    private static readonly Func<string, string> _normalForm = e => e.ToLowerInvariant();
    public EmailAddressFromValidator(IOptions<AppConfiguration> options)
    {
        var senders = options.Value.EmailSenders;
        var allowedDomainNames = senders.Mcs
            .Union(senders.Mpss)
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
            .WithMessage($"Allowed domain names for sender: {string.Join(',', allowedDomainNames)}.");
    }
}