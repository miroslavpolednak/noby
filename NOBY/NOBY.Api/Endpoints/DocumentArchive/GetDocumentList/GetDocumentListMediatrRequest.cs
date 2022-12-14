namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListMediatrRequest:IRequest<GetDocumentListResponse>
{
	public GetDocumentListMediatrRequest(long caseId)
	{
        CaseId = caseId;
    }

    public long CaseId { get; }
}
