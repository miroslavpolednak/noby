using NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto;
using NOBY.Api.Endpoints.Cases.GetTaskList.Dto;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

namespace NOBY.Api.Endpoints.Cases.GetTaskDetail;

public class GetTaskDetailResponse
{
    public WorkflowTask? Task { get; set; }
    
    public WorkflowTaskDetail? TaskDetail { get; set; }
    
    public List<DocumentsMetadata>? Documents { get; set; }
}