﻿using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class PartyValidator : AbstractValidator<Party>
{
    public PartyValidator()
    {
        RuleFor(party => party)
            .Must(party =>
                (party.LegalPerson is not null && party.NaturalPerson is null) ||
                (party.LegalPerson is null && party.NaturalPerson is not null))
            .WithErrorCode(ErrorCodes.EmailParty.EitherLegalOrNaturalPersonRequired)
            .WithMessage($"{nameof(Party)} must contain either {nameof(LegalPerson)} or {nameof(NaturalPerson)}.");

        When(party => party.LegalPerson is not null, () =>
        {
            RuleFor(party => party.LegalPerson!)
                .SetValidator(new LegalPersonValidator())
                    .WithErrorCode(ErrorCodes.EmailParty.LegalPersonInvalid)
                    .WithMessage($"Invalid {nameof(Party.LegalPerson)}.");
        });

        When(party => party.LegalPerson is not null, () =>
        {
            RuleFor(party => party.NaturalPerson!)
                .SetValidator(new NaturalPersonValidator())
                    .WithErrorCode(ErrorCodes.EmailParty.NaturalPersonInvalid)
                    .WithMessage($"Invalid {nameof(Party.NaturalPerson)}.");
        });
    }
}