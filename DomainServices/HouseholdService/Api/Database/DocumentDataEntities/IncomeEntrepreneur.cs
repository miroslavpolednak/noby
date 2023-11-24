namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities;

internal sealed class IncomeEntrepreneur
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public string? Cin { get; set; }
    public string? BirthNumber { get; set; }
    public int? CountryOfResidenceId { get; set; }
}
