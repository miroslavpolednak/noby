namespace NOBY.Api.Endpoints.Workflow.GetTaskList;

internal sealed record GetTaskListRequest(long CaseId)
    : IRequest<GetTaskListResponse>
{
}
