namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSADetail;

public class GetDocumentOnSADetailRequest : IRequest<DocumentOnSaGetDocumentOnSADetailResponse>
{
    public GetDocumentOnSADetailRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }

    public int DocumentOnSAId { get; }
}
