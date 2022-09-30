using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomer;

internal class UpdateCustomerMediatrRequestValidator
    : AbstractValidator<UpdateCustomerMediatrRequest>
{
    static DateTime _dateOfBirthMin = new DateTime(1900, 1, 1);

    public UpdateCustomerMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("16024");

        RuleFor(t => t.Request.Customer.DateOfBirthNaturalPerson)
            .Must(d => d > _dateOfBirthMin && d < DateTime.Now)
            .WithMessage("Date of birth is out of range").WithErrorCode("16038")
            .When(t => t.Request.Customer.DateOfBirthNaturalPerson is not null);
    }
}