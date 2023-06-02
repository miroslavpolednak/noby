namespace NOBY.Api.Endpoints.Workflow.Dto;

public class TaskCommunicationItem
{
    /// <summary>
    /// Text požadavku
    /// </summary>
    /// <example>Prosím o součinnost.</example>
    public string TaskRequest { get; set; } = null!;

    /// <summary>
    /// Dožádání: odpověď
    /// </summary>
    /// <example>Posílám Vám požadované dokumenty.</example>
    public string TaskResponse { get; set; } = null!;
}