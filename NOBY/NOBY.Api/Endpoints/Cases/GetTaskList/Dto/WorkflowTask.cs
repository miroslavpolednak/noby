namespace NOBY.Api.Endpoints.Cases.GetTaskList.Dto;

public sealed class WorkflowTask
{
    public long TaskId { get; set; }

    public long TaskProcessId { get; set; }

    public int TypeId { get; set; }

    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public int StateId { get; set; }

    public DateTime CreatedOn { get; set; }
}