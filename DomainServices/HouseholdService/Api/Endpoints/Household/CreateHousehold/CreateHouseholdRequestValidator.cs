using CIS.Foms.Enums;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.CreateHousehold;

internal sealed class CreateHouseholdRequestValidator
    : AbstractValidator<Contracts.CreateHouseholdRequest>
{
    public CreateHouseholdRequestValidator(
        Database.HouseholdServiceDbContext dbContext, 
        CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("16010");

        RuleFor(t => t.HouseholdTypeId)
            .GreaterThan(0)
            .WithMessage("HouseholdTypeId must be > 0").WithErrorCode("16027");

        RuleFor(t => t.HouseholdTypeId)
            .Must(t => (HouseholdTypes)t != HouseholdTypes.Unknown)
            .WithMessage("HouseholdTypeId must be > 0").WithErrorCode("16027");

        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithMessage("CustomerOnSAId1 is not set although CustomerOnSAId2 is.").WithErrorCode("16056");

        // Main domacnost muze byt jen jedna. Pocitame, ze Main domacnost je zalozena vzdy na zacatku, takze pokud uz v tuhle chvili nejaka existuje, tak je to spatne.
        RuleFor(t => t.SalesArrangementId)
            .Must(saId => !dbContext.Households.Any(t => t.SalesArrangementId == saId))
            .WithMessage("Only one Debtor household allowed").WithErrorCode("16031")
            .When(t => t.HouseholdTypeId == (int)HouseholdTypes.Main);

        // check household role
        RuleFor(t => t.HouseholdTypeId)
            .Must(householdTypeId => codebookService.HouseholdTypesSynchronous()?.Any(t => t.Id == householdTypeId) ?? false)
            .WithMessage(request => $"HouseholdTypeId {request.HouseholdTypeId} does not exist.").WithErrorCode("16023");

    }
}