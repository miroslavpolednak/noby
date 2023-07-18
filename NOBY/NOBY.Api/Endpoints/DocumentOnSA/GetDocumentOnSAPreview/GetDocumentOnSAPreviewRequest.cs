namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAPreview;

public class GetDocumentOnSAPreviewRequest : IRequest<GetDocumentOnSAPreviewResponse>
{
    public int SalesArrangementId { get; }
    
    public int DocumentOnSAId { get; }
    
    public GetDocumentOnSAPreviewRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }
}