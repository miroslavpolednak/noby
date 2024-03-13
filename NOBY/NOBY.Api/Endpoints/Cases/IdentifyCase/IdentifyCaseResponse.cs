namespace NOBY.Api.Endpoints.Cases.IdentifyCase;

public sealed class IdentifyCaseResponse
{
    /// <summary>
    /// Nalezene case-s
    /// </summary>
    public List<Dto.IdentifyCaseResponseItem>? Cases { get; set; }
    
    public NOBY.Dto.Workflow.WorkflowTask? Task { get; set; }
    
    public NOBY.Dto.Workflow.WorkflowTaskDetail? TaskDetail { get; set; }
    
    public List<NOBY.Dto.Documents.DocumentsMetadata>? Documents { get; set; }
}