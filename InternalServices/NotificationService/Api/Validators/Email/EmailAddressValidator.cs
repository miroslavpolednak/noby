using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress.Value)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.EmailAddress.ValueRequired)
                .WithMessage($"{nameof(EmailAddress.Value)} required.")
            .EmailAddress()
                .WithErrorCode(ErrorCodes.Validation.EmailAddress.ValueInvalid)
                .WithMessage($"Invalid {nameof(EmailAddress.Value)}.");

        RuleFor(emailAddress => emailAddress.Party)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.EmailAddress.PartyRequired)
                .WithMessage($"{nameof(EmailAddress.Party)} required.")
            .SetValidator(new PartyValidator())
                .WithErrorCode(ErrorCodes.Validation.EmailAddress.PartyInvalid)
                .WithMessage($"Invalid {nameof(EmailAddress.Party)}");
    }
}