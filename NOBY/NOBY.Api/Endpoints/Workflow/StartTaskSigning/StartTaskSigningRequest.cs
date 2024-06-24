namespace NOBY.Api.Endpoints.Workflow.StartTaskSigning;

internal sealed record StartTaskSigningRequest(long CaseId, long TaskId)
    : IRequest<WorkflowStartTaskSigningResponse>
{
}