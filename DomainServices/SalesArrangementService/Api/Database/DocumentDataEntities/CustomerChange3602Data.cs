using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class CustomerChange3602Data : IDocumentData
{
    int IDocumentData.Version => 1;

    public int HouseholdId { get; set; }

    public bool? IsSpouseInDebt { get; set; }
}