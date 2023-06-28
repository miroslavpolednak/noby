using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
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
            .WithErrorCode(ErrorHandling.ErrorCodeMapper.EitherLegalOrNaturalPersonRequired);

        When(party => party.LegalPerson is not null, () =>
        {
            RuleFor(party => party.LegalPerson!)
                .SetValidator(new LegalPersonValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.LegalPersonInvalid);
        });

        When(party => party.NaturalPerson is not null, () =>
        {
            RuleFor(party => party.NaturalPerson!)
                .SetValidator(new NaturalPersonValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.NaturalPersonInvalid);
        });
    }
}