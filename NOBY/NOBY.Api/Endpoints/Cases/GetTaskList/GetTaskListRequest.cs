namespace NOBY.Api.Endpoints.Cases.GetTaskList;

internal record GetTaskListRequest(long CaseId)
    : IRequest<GetTaskListResponse>
{
}
