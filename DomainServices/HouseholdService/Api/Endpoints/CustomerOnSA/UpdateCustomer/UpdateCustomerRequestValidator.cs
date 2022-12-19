using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomer;

internal sealed class UpdateCustomerRequestValidator
    : AbstractValidator<UpdateCustomerRequest>
{
    static DateTime _dateOfBirthMin = new DateTime(1900, 1, 1);

    public UpdateCustomerRequestValidator()
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("16024");

        RuleFor(t => t.Customer.DateOfBirthNaturalPerson)
            .Must(d => d > _dateOfBirthMin && d < DateTime.Now)
            .WithMessage("Date of birth is out of range").WithErrorCode("16038")
            .When(t => t.Customer.DateOfBirthNaturalPerson is not null);
    }
}