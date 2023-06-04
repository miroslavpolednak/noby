namespace NOBY.Api.Endpoints.Workflow.GetCurrentPriceException;

public sealed class GetCurrentPriceExceptionResponse
{
    public Dto.Workflow.WorkflowTask? Task { get; set; } = null!;

    public Dto.Workflow.WorkflowTaskDetail? TaskDetail { get; set; } = null!;

    public List<Dto.Documents.DocumentsMetadata>? Documents { get; set; }
}
