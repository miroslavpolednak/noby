using FluentValidation;

namespace DomainServices.HouseholdService.Api.Handlers.Household.LinkCustomerOnSAToHousehold;

internal class LinkCustomerOnSAToHouseholdMediatrRequestValidator
    : AbstractValidator<LinkCustomerOnSAToHouseholdMediatrRequest>
{
    public LinkCustomerOnSAToHouseholdMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CustomerOnSAId1)
            .NotNull()
            .When(t => t.Request.CustomerOnSAId2.HasValue)
            .WithMessage("CustomerOnSAId1 is not set although CustomerOnSAId2 is.").WithErrorCode("16056");
    }
}
