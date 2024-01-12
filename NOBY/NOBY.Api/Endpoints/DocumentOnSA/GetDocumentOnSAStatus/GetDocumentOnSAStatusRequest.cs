namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAStatus;

public class GetDocumentOnSAStatusRequest : IRequest<GetDocumentOnSAStatusResponse>
{
    public GetDocumentOnSAStatusRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }
    public int DocumentOnSAId { get; }
}
