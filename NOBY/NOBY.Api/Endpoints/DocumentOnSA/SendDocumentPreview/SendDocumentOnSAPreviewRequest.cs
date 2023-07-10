namespace NOBY.Api.Endpoints.DocumentOnSA.SendDocumentPreview;

public class SendDocumentOnSAPreviewRequest : IRequest
{
    public int SalesArrangementId { get; }
    
    public int DocumentOnSAId { get; }
    
    public SendDocumentOnSAPreviewRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }
}