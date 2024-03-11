namespace NOBY.Api.Endpoints.Cases.IdentifyCase;

public sealed class IdentifyCaseResponse
{
    /// <summary>
    /// Nalezene case-s
    /// </summary>
    public List<IdentifyCaseResponseItem>? Cases { get; set; }
    
    public NOBY.Dto.Workflow.WorkflowTask? Task { get; set; }
    
    public NOBY.Dto.Workflow.WorkflowTaskDetail? TaskDetail { get; set; }
    
    public List<NOBY.Dto.Documents.DocumentsMetadata>? Documents { get; set; }
}

public sealed record IdentifyCaseResponseItem
{
    /// <summary>
    /// ID obchodního případu
    /// </summary>
    public long CaseId { get; set; }

    /// <summary>
    /// Typ vztahu
    /// </summary>
    public int? ContractRelationshipTypeId { get; set; }

    public IdentifyCaseResponseItem(long caseId)
    {
        CaseId = caseId;
    }
}