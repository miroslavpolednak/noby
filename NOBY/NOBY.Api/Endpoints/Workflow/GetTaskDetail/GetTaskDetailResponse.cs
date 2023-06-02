using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.Workflow.Dto;

namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

public class GetTaskDetailResponse
{
    public WorkflowTask? Task { get; set; }

    public WorkflowTaskDetail? TaskDetail { get; set; }

    public List<DocumentsMetadata>? Documents { get; set; }
}