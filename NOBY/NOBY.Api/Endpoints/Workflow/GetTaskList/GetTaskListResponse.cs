using NOBY.Api.Endpoints.Workflow.Dto;

namespace NOBY.Api.Endpoints.Workflow.GetTaskList;

public sealed class GetTaskListResponse
{
    public List<WorkflowTask>? Tasks { get; set; }

    public List<Dto.WorkflowProcess> Processes { get; set; } = new();
}
