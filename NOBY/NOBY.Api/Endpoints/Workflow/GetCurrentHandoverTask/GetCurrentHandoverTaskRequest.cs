namespace NOBY.Api.Endpoints.Workflow.GetCurrentHandoverTask;

internal sealed record GetCurrentHandoverTaskRequest(long CaseId)
    : IRequest<WorkflowGetCurrentHandoverTaskResponse>
{
}
