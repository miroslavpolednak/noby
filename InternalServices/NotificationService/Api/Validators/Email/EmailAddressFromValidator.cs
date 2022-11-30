using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailAddressFromValidator : AbstractValidator<EmailAddress>
{
    private static readonly HashSet<string> _allowedHost = new() { "kb.cz", "mpss.cz" };

    public EmailAddressFromValidator()
    {
        RuleFor(emailAddress => emailAddress)
            .SetValidator(new EmailAddressValidator());
        
        RuleFor(emailAddress => emailAddress.Value)
            .Must(email =>
            {
                var host = email.ToLowerInvariant().Split('@').Last();
                return _allowedHost.Contains(host);
            })
            .WithErrorCode(ErrorCodes.SendEmail.FromInvalid)
            .WithMessage($"Allowed host for sender: {string.Join(',', _allowedHost)}.");
    }
}