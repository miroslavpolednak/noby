namespace NOBY.Api.Endpoints.Workflow.StartTaskSigning;

public sealed class StartTaskSigningRequest : IRequest<StartTaskSigningResponse>
{
    public long CaseId { get; }
    public long TaskId { get; }
    
    public StartTaskSigningRequest(long caseId, long taskId)
    {
        CaseId = caseId;
        TaskId = taskId;
    }
}