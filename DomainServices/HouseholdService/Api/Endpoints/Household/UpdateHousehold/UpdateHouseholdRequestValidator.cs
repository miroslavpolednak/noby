using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.UpdateHousehold;

internal sealed class UpdateHouseholdRequestValidator
    : AbstractValidator<UpdateHouseholdRequest>
{
    public UpdateHouseholdRequestValidator()
    {
        RuleFor(t => t.HouseholdId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.HouseholdIdIsEmpty);

        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithErrorCode(ErrorCodeMapper.Customer2WithoutCustomer1);
    }
}