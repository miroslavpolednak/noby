namespace NOBY.Api.Endpoints.Cases.GetTaskList;

public sealed class GetTaskListResponseNew
{
    public List<Dto.WorkflowTaskNew>? Tasks { get; set; }

    public List<Dto.WorkflowProcess>  Processes { get; set; } = new();
}
