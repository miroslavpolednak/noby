using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators;

internal sealed class CustomerOnSAValidator
    : AbstractValidator<Contracts.CustomerOnSABase>
{
    public CustomerOnSAValidator()
    {
        When(r => r.DateOfBirthNaturalPerson is not null,
             () =>
             {
                 RuleFor(r => (DateTime)r.DateOfBirthNaturalPerson)
                     .InclusiveBetween(new DateTime(1850, 1, 1), DateTime.Today)
                     .WithErrorCode(ErrorCodeMapper.InvalidDateOfBirth);
             });
    }
}