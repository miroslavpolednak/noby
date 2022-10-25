using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class NaturalPersonValidator : AbstractValidator<NaturalPerson>
{
    public NaturalPersonValidator()
    {
        RuleFor(person => person.FirstName)
            .NotEmpty()
            .MaximumLength(40)
            .WithErrorCode(nameof(NaturalPerson.FirstName));

        RuleFor(person => person.MiddleName)
            .MaximumLength(40)
            .WithErrorCode(nameof(NaturalPerson.MiddleName));

        RuleFor(person => person.Surname)
            .NotEmpty()
            .MaximumLength(80)
            .WithErrorCode(nameof(NaturalPerson.Surname));
    }
}