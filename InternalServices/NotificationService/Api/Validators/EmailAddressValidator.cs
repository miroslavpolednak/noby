using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

internal sealed class EmailAddressValidator 
    : AbstractValidator<Contracts.v2.SendEmailRequest.Types.EmailAddress>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress.Value)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.ValueRequired)
            .EmailAddress()
                .WithErrorCode(ErrorCodeMapper.ValueInvalid);

        When(emailAddress => emailAddress.Party?.LegalPerson is not null, () =>
        {
            RuleFor(emailAddress => emailAddress.Party.LegalPerson.Name)
                .NotEmpty()
                    .WithErrorCode(ErrorCodeMapper.NameRequired)
                .MaximumLength(255)
                    .WithErrorCode(ErrorCodeMapper.NameLengthLimitExceeded);
        });

        When(emailAddress => emailAddress.Party?.NaturalPerson is not null, () =>
        {
            RuleFor(emailAddress => emailAddress.Party.NaturalPerson.FirstName)
                .NotEmpty()
                    .WithErrorCode(ErrorCodeMapper.FirstNameRequired)
                .MaximumLength(40)
                    .WithErrorCode(ErrorCodeMapper.FirstNameLengthLimitExceeded);

            RuleFor(emailAddress => emailAddress.Party.NaturalPerson.MiddleName)
                .MaximumLength(40)
                    .WithErrorCode(ErrorCodeMapper.MiddleNameLengthLimitExceeded);

            RuleFor(emailAddress => emailAddress.Party.NaturalPerson.Surname)
                .NotEmpty()
                    .WithErrorCode(ErrorCodeMapper.SurnameRequired)
                .MaximumLength(80)
                    .WithErrorCode(ErrorCodeMapper.SurnameLengthLimitExceeded);
        });
    }
}
