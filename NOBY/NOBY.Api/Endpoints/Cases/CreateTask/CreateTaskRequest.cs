using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.Cases.CreateTask;

public sealed class CreateTaskRequest
    : IRequest<long>
{
    [JsonIgnore]
    internal long CaseId;

    internal CreateTaskRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }

    /// <summary>
    /// ID typu úkolu (Předání na specialistu = 7, Konzultace = 3)
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

    public List<DocumentInformation>? Attachments { get; set; }
}
