namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAData;

public class GetDocumentOnSADataRequest:IRequest<GetDocumentOnSADataResponse>
{
	public GetDocumentOnSADataRequest(int salesArrangementId, int documentOnSAId)
	{
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }
    public int DocumentOnSAId { get; }
}
