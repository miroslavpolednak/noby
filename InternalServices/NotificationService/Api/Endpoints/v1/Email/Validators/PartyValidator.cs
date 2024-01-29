using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email.Validators;

public class PartyValidator : AbstractValidator<Party>
{
    public PartyValidator()
    {
        RuleFor(party => party)
            .Must(party =>
                (party.LegalPerson is not null && party.NaturalPerson is null) ||
                (party.LegalPerson is null && party.NaturalPerson is not null))
            .WithErrorCode(ErrorCodeMapper.EitherLegalOrNaturalPersonRequired);

        When(party => party.LegalPerson is not null, () =>
        {
            RuleFor(party => party.LegalPerson!)
                .SetValidator(new LegalPersonValidator())
                    .WithErrorCode(ErrorCodeMapper.LegalPersonInvalid);
        });

        When(party => party.NaturalPerson is not null, () =>
        {
            RuleFor(party => party.NaturalPerson!)
                .SetValidator(new NaturalPersonValidator())
                    .WithErrorCode(ErrorCodeMapper.NaturalPersonInvalid);
        });
    }
}