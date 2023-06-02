namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

public sealed class GetTaskDetailResponse
{
    public Workflow.Dto.WorkflowTask? Task { get; set; }

    public Workflow.Dto.WorkflowTaskDetail? TaskDetail { get; set; }

    public List<DocumentArchive.GetDocumentList.DocumentsMetadata>? Documents { get; set; }
}