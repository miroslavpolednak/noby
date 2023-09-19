namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSA;

public class GetDocumentOnSARequest:IRequest<GetDocumentOnSAResponse>
{
	public GetDocumentOnSARequest(int salesArrangementId, int documentOnSAId)
	{
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }
    public int DocumentOnSAId { get; }
}
