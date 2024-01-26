namespace NOBY.Api.Endpoints.Workflow.GetCurrentHandoverTask;

public class GetCurrentHandoverTaskRequest : IRequest<GetCurrentHandoverTaskResponse>
{
    public GetCurrentHandoverTaskRequest(long caseId)
    {
        CaseId = caseId;
    }

    public long CaseId { get; }
}
