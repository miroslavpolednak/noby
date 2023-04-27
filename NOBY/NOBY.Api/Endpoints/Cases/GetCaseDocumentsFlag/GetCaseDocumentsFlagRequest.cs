namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

public class GetCaseDocumentsFlagRequest : IRequest<GetCaseDocumentsFlagResponse>
{
    public GetCaseDocumentsFlagRequest(long caseId)
    {
        CaseId = caseId;
    }

    public long CaseId { get; }
}
