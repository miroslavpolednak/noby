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
            .WithErrorCode(ValidationMessages.SalesArrangementIdIsEmpty);

        RuleFor(t => t.HouseholdTypeId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.HouseholdTypeIdIsEmpty)
            .Must(t => (HouseholdTypes)t != HouseholdTypes.Unknown)
            .WithErrorCode(ValidationMessages.HouseholdTypeIdIsEmpty);

        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .WithErrorCode(ValidationMessages.Customer2WithoutCustomer1)
            .When(t => t.CustomerOnSAId2.HasValue);

        // Main domacnost muze byt jen jedna. Pocitame, ze Main domacnost je zalozena vzdy na zacatku, takze pokud uz v tuhle chvili nejaka existuje, tak je to spatne.
        RuleFor(t => t.SalesArrangementId)
            .Must(saId => !dbContext.Households.Any(t => t.SalesArrangementId == saId))
            .WithErrorCode(ValidationMessages.MoreDebtorHouseholds)
            .When(t => t.HouseholdTypeId == (int)HouseholdTypes.Main);

        // check household role
        RuleFor(t => t.HouseholdTypeId)
            .MustAsync(async (householdTypeId, cancellationToken) => (await codebookService.HouseholdTypes(cancellationToken)).Any(t => t.Id == householdTypeId))
            .WithErrorCode(ValidationMessages.HouseholdTypeIdNotFound);
    }
}