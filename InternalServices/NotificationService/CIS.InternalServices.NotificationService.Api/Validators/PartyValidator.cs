using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class PartyValidator : AbstractValidator<Party>
{
    public PartyValidator()
    {
        RuleFor(party => party)
            .Must(party =>
                (party.LegalPerson is not null && party.NaturalPerson is null) ||
                (party.LegalPerson is null && party.NaturalPerson is not null));

        When(party => party.LegalPerson is not null, () =>
        {
            RuleFor(party => party.LegalPerson!)
                .SetValidator(new LegalPersonValidator())
                .WithErrorCode(nameof(Party.LegalPerson));
        });

        When(party => party.LegalPerson is not null, () =>
        {
            RuleFor(party => party.NaturalPerson!)
                .SetValidator(new NaturalPersonValidator())
                .WithErrorCode(nameof(Party.NaturalPerson));
        });
    }
}