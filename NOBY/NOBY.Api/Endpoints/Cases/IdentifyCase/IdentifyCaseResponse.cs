namespace NOBY.Api.Endpoints.Cases.IdentifyCase;

public sealed class IdentifyCaseResponse
{
    /// <summary>
    /// ID obchodního případu
    /// </summary>
    /// <example>24777</example>
    public long? CaseId { get; set; }
    
    public NOBY.Dto.Workflow.WorkflowTask? Task { get; set; }
    
    public NOBY.Dto.Workflow.WorkflowTaskDetail? TaskDetail { get; set; }
    
    public List<NOBY.Dto.Documents.DocumentsMetadata>? Documents { get; set; }
}