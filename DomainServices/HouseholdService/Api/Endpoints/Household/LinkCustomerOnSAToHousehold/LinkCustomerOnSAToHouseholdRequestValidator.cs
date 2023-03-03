using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.LinkCustomerOnSAToHousehold;

internal sealed class LinkCustomerOnSAToHouseholdRequestValidator
    : AbstractValidator<LinkCustomerOnSAToHouseholdRequest>
{
    public LinkCustomerOnSAToHouseholdRequestValidator()
    {
        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithErrorCode(ErrorCodeMapper.Customer2WithoutCustomer1);
    }
}
