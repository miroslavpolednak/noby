namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSADetail;

public class GetDocumentOnSADetailRequest : IRequest<GetDocumentOnSADetailResponse>
{
    public GetDocumentOnSADetailRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }

    public int DocumentOnSAId { get; }
}
