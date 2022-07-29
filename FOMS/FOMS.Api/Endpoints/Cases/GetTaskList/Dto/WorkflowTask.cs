namespace FOMS.Api.Endpoints.Cases.GetTaskList.Dto;

public sealed class WorkflowTask
{
    public int TaskId { get; set; }
    public int TaskProcessId { get; set; }
    public int TypeId { get; set; }
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public int StateId { get; set; }
    public DateTime CreatedOn { get; set; }
}
