namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListRequest:IRequest<GetDocumentsSignListResponse>
{
	public GetDocumentsSignListRequest(int salesArrangementId)
	{
        SalesArrangementId = salesArrangementId;
    }

    public int SalesArrangementId { get; }
}
