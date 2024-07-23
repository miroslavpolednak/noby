namespace NOBY.Api.Endpoints.DocumentOnSA.RefreshElectronicDocument;

public class RefreshElectronicDocumentRequest : IRequest<DocumentOnSaRefreshElectronicDocumentResponse>
{
    public RefreshElectronicDocumentRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }

    public int DocumentOnSAId { get; }
}
