namespace NOBY.Api.Endpoints.Cases.GetTaskDetail;

public class GetTaskDetailRequest : IRequest<GetTaskDetailResponse>
{
    public long CaseId { get; }
    public int TaskId { get; }

    public GetTaskDetailRequest(long caseId, int taskId)
    {
        CaseId = caseId;
        TaskId = taskId;
    }
}