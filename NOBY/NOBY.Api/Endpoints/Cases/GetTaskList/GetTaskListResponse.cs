namespace NOBY.Api.Endpoints.Cases.GetTaskList;

public sealed class GetTaskListResponse
{
    public List<Dto.WorkflowTask>? Tasks { get; set; }
}
