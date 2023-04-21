namespace NOBY.Api.Endpoints.Cases.GetTaskList;

internal sealed record GetTaskListRequest(long CaseId)
    : IRequest<GetTaskListResponse>
{
}
