namespace NOBY.Api.Endpoints.Cases.GetTaskList.Dto;

public class WorkflowProcess
{
    /// <summary>
    /// Noby proces ID. Jde o ID sady úkolů generované Starbuildem.
    /// </summary>
    /// <example>22777</example>
    public int ProcessId { get; set; }

    /// <summary>
    /// Datum vytvoření procesu
    /// </summary>
    /// <example>12.12.2021</example>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Jméno typu procesu (nezkrácené)
    /// </summary>
    /// <example>Hlavní úvěrový proces</example>
    public string? ProcessNameLong { get; set; }

    /// <summary>
    /// Název Noby stavu procesu
    /// </summary>
    /// <example>SPRÁVA</example>
    public string StateName { get; set; } = null!;

    /// <summary>
    /// Indikátor barvy Noby stavu
    /// </summary>
    /// <example>Active</example>
    public StateIndicator StateIndicator { get; set; }
}