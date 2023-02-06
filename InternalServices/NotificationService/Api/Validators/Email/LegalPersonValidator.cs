using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class LegalPersonValidator : AbstractValidator<LegalPerson>
{
    public LegalPersonValidator()
    {
        RuleFor(person => person.Name)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Validation.LegalPerson.NameRequired)
                .WithMessage($"{nameof(LegalPerson.Name)} required.")
            .MaximumLength(255)
                .WithErrorCode(ErrorCodes.Validation.LegalPerson.NameLengthLimitExceeded)
                .WithMessage($"Maximum length of {nameof(LegalPerson.Name)} is 255.");
    }
}