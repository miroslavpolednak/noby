using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress.Value)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ValueRequired)
            .EmailAddress()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ValueInvalid);

        RuleFor(emailAddress => emailAddress.Party)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.PartyRequired)
            .SetValidator(new PartyValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.PartyInvalid);
    }
}