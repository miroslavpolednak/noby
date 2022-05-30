namespace FOMS.Api.Endpoints.Cases.GetTaskList;

internal record GetTaskListRequest(long CaseId)
    : IRequest<GetTaskListResponse>
{
}
