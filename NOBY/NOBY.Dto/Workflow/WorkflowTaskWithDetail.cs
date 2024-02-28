namespace NOBY.Dto.Workflow;

internal sealed class WorkflowTaskWithDetail
{
    public WorkflowTask? Task { get; set; }

    public WorkflowTaskDetail? TaskDetail { get; set; }

    public List<Documents.DocumentsMetadata>? Documents { get; set; }
}
