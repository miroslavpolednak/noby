using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.LinkCustomerOnSAToHousehold;

internal class LinkCustomerOnSAToHouseholdRequestValidator
    : AbstractValidator<LinkCustomerOnSAToHouseholdRequest>
{
    public LinkCustomerOnSAToHouseholdRequestValidator()
    {
        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithMessage("CustomerOnSAId1 is not set although CustomerOnSAId2 is.").WithErrorCode("16056");
    }
}
