using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Workflow.CreateTask;

public sealed class CreateTaskRequest
    : IRequest<long>
{
    [JsonIgnore]
    internal long CaseId;

    internal CreateTaskRequest InfuseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }

    /// <summary>
    /// ID typu úkolu (Cenová výjimka = 2, Konzultace = 3, Předání na specialistu = 7)
    /// </summary>
    /// <example>7</example>
    [Required]
    public int TaskTypeId { get; set; }

    /// <summary>
    /// ID typu konzultace
    /// </summary>
    /// <example>0</example>
    public int? TaskSubtypeId { get; set; }

    /// <summary>
    /// ID nadřazeného procesu
    /// </summary>
    /// <example>24556</example>
    [Required]
    public long ProcessId { get; set; }

    /// <summary>
    /// Text požadavku
    /// </summary>
    /// <example>Prosím o pomoc s tímto případem.</example>
    [Required]
    public string TaskUserRequest { get; set; } = string.Empty;

    public List<NOBY.Dto.Documents.DocumentInformation>? Attachments { get; set; }
}
