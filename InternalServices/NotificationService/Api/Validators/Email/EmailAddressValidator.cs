using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress.Value)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.EmailAddress.ValueRequired)
                .WithMessage($"{nameof(EmailAddress.Value)} required.")
            .EmailAddress()
                .WithErrorCode(ErrorCodes.EmailAddress.ValueInvalid)
                .WithMessage($"Invalid {nameof(EmailAddress.Value)}.");

        RuleFor(emailAddress => emailAddress.Party)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.EmailAddress.PartyRequired)
                .WithMessage($"{nameof(EmailAddress.Party)} required.")
            .Must(party =>
                    (party.LegalPerson is not null && party.NaturalPerson is null) ||
                    (party.LegalPerson is null && party.NaturalPerson is not null))
                .WithErrorCode(ErrorCodes.EmailAddress.PartyInvalid)
                .WithMessage($"{nameof(EmailAddress.Party)} must contain either {nameof(LegalPerson)} or {nameof(NaturalPerson)}.");
    }
}