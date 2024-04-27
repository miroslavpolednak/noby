using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class RefixationData : IDocumentData
{
    int IDocumentData.Version => 1;

    public bool? ManagedByRC2 { get; set; }
    public string? Comment { get; set; }
    public string? IndividualPriceCommentLastVersion { get; set; }
}


internal sealed class ExtraPaymentData : IDocumentData
{
    public int Version => 1;

    public string? IndividualPriceCommentLastVersion { get; set; }

    public int? HandoverTypeDetailId { get; set; }

    public int? ClientKBId { get; set; }
}