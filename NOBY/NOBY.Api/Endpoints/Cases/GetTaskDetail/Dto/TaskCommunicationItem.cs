namespace NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto;

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