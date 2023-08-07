namespace NOBY.Api.Endpoints.Cases.CancelCase;

public class CancelCaseRequest : IRequest<CancelCaseResponse>
{
    public long CaseId { get; }

    public CancelCaseRequest(long caseId)
    {
        CaseId = caseId;
    }
}