namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListRequest:IRequest<GetDocumentListResponse>
{
	public GetDocumentListRequest(long caseId)
	{
        CaseId = caseId;
    }

    public long CaseId { get; }
}
