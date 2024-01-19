using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email.Validators;

public class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress.Value)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ValueRequired)
            .EmailAddress()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ValueInvalid);

        When(emailAddress => emailAddress.Party is not null, () =>
        {
            RuleFor(emailAddress => emailAddress.Party!)
                .SetValidator(new PartyValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.PartyInvalid);
        });
    }
}