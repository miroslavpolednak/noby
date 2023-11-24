namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities;

internal sealed class IncomeOther
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public int? IncomeOtherTypeId { get; set; }
}
