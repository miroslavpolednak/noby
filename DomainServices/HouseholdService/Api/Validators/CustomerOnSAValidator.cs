using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators;

internal sealed class CustomerOnSAValidator
    : AbstractValidator<Contracts.CustomerOnSABase>
{
    private static DateTime _dateOfBirthMin = new DateTime(1900, 1, 1);

    public CustomerOnSAValidator()
    {
        RuleFor(t => t.DateOfBirthNaturalPerson)
            .Must(d => d > _dateOfBirthMin && d < DateTime.Now)
            .WithErrorCode(ErrorCodeMapper.InvalidDateOfBirth)
            .When(t => t.DateOfBirthNaturalPerson is not null);
    }
}