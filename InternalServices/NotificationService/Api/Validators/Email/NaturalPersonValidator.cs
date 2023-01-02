using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class NaturalPersonValidator : AbstractValidator<NaturalPerson>
{
    public NaturalPersonValidator()
    {
        RuleFor(person => person.FirstName)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.NaturalPerson.FirstNameRequired)
                .WithMessage($"{nameof(NaturalPerson.FirstName)} required.")
            .MaximumLength(40)
                .WithErrorCode(ErrorCodes.NaturalPerson.FirstNameLengthLimitExceeded)
                .WithMessage($"Maximum length of {nameof(NaturalPerson.FirstName)} is 40.");

        RuleFor(person => person.MiddleName)
            .MaximumLength(40)
                .WithErrorCode(ErrorCodes.NaturalPerson.MiddleNameLengthLimitExceeded)
                .WithMessage($"Maximum length of {nameof(NaturalPerson.MiddleName)} is 40.")
            .WithErrorCode(nameof(NaturalPerson.MiddleName));

        RuleFor(person => person.Surname)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.NaturalPerson.SurnameRequired)
                .WithMessage($"{nameof(NaturalPerson.Surname)} required.")
            .MaximumLength(80)
                .WithErrorCode(ErrorCodes.NaturalPerson.SurnameLengthLimitExceeded)
                .WithMessage($"Maximum length of {nameof(NaturalPerson.Surname)} is 80.");
    }
}