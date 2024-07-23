using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class ExtraPaymentData 
    : IDocumentData
{
    public int Version => 1;

    public string? IndividualPriceCommentLastVersion { get; set; }

    public int? HandoverTypeDetailId { get; set; }

    public long? ClientKBId { get; set; }

    public string? ClientFirstName { get; set; }

    public string? ClientLastName { get; set; }
}
