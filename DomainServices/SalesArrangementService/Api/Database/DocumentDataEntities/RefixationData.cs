using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class RefixationData : IDocumentData
{
    int IDocumentData.Version => 1;

    public bool? ManagedByRC2 { get; set; }
    public string? Comment { get; set; }
    public string? IndividualPriceCommentLastVersion { get; set; }
}
