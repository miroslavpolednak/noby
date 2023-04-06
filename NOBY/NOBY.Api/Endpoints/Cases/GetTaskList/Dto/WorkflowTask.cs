namespace NOBY.Api.Endpoints.Cases.GetTaskList.Dto;

public sealed class WorkflowTask
{
    /// <summary>
    /// Noby task ID. Jde o ID sady úkolů generované Starbuildem.
    /// </summary>
    /// <example>22777</example>
    public int TaskId { get; set; }

    /// <summary>
    /// Datum vytvoření úkolu
    /// </summary>
    /// <example>12.12.2022</example>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// ID typu úkolu podle <a href="https://wiki.kb.cz/display/HT/WorkflowTaskProcessType+%28CIS_WFL_CISELNIKY_HODNOTY+s+CISELNIK_ID+%3D+10%29+-+MOCK">číselníku typu úkolů</a>
    /// </summary>
    /// <example>1</example>
    public int TaskTypeId { get; set; }

    /// <summary>
    /// Jméno typu úkolu
    /// </summary>
    /// <example>Dožádání</example>
    public string? TaskTypeName { get; set; }

    /// <summary>
    /// Označení úkolu (název podtypu úkolu)
    /// </summary>
    /// <example>Obecné</example>
    public string TaskSubtypeName { get; set; } = null!;

    /// <summary>
    /// ID procesu
    /// </summary>
    /// <example>2344</example>
    public int ProcessId { get; set; }

    /// <summary>
    /// Jméno typu procesu (zkrácené)
    /// </summary>
    /// <example>Hlavní</example>
    public string ProcessNameShort { get; set; } = null!;

    /// <summary>
    /// ID Noby stavu úkolu podle <a href="https://wiki.kb.cz/display/HT/WorkflowTaskStateNoby">číselníku stavů úkolů</a>
    /// </summary>
    /// <example>1</example>
    public int StateId { get; set; }

    /// <summary>
    /// Název Noby stavu úkolu
    /// </summary>
    /// <example>K VYŘÍZENÍ</example>
    public string StateName { get; set; } = null!;

    /// <summary>
    /// Filter Noby stavu úkolu
    /// </summary>
    /// <example>Active</example>
    public StateFilter StateFilter { get; set; }

    /// <summary>
    /// Indikátor barvy Noby stavu
    /// </summary>
    /// <example>Active</example>
    public StateIndicator StateIndicator { get; set; }
}
