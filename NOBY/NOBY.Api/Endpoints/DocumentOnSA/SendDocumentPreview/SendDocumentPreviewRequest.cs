namespace NOBY.Api.Endpoints.DocumentOnSA.SendDocumentPreview;

public class SendDocumentPreviewRequest : IRequest
{
    public int SalesArrangementId { get; }
    
    public int DocumentOnSAId { get; }
    
    public SendDocumentPreviewRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }
}