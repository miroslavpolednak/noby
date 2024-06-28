namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

internal sealed record GetTaskDetailRequest(long CaseId, long TaskId)
    : IRequest<WorkflowGetTaskDetailResponse>
{
}