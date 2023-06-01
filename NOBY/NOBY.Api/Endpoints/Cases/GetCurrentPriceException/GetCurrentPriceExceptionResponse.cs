namespace NOBY.Api.Endpoints.Cases.GetCurrentPriceException;

public sealed class GetCurrentPriceExceptionResponse
{
    public Dto.WorkflowTask Task { get; set; } = null!;
    
    public Dto.WorkflowTaskDetail TaskDetail { get; set; } = null!;

    public List<DocumentArchive.GetDocumentList.DocumentsMetadata>? Documents { get; set; }
}
