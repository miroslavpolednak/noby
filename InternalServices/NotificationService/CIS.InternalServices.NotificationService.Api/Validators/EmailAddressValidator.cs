﻿using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress.Value)
            .EmailAddress()
            .WithErrorCode(nameof(EmailAddress.Value));

        RuleFor(emailAddress => emailAddress.Party)
            .Cascade(CascadeMode.Stop)
            .Must(party =>
                (party.LegalPerson is not null && party.NaturalPerson is null) ||
                (party.LegalPerson is null && party.NaturalPerson is not null))
            .WithMessage($"Either {nameof(LegalPerson)} or {nameof(NaturalPerson)} required.");
    }
}