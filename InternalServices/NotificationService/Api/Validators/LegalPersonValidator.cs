using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class LegalPersonValidator : AbstractValidator<LegalPerson>
{
    public LegalPersonValidator()
    {
        RuleFor(person => person.Name)
            .NotEmpty()
            .MaximumLength(255)
            .WithErrorCode(nameof(LegalPerson.Name));
    }
}