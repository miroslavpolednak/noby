using NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.Amendments;

namespace NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto;

public class WorkflowTaskDetail
{
    /// <summary>
    /// SB ID úkolu, není zobrazeno na FE UI
    /// </summary>
    /// <example>22456</example>
    public int TaskIdSB { get; set; }

    /// <summary>
    /// Login zadavatele/zpracovatele
    /// </summary>
    /// <example>14</example>
    public string? PerformerLogin { get; set; }
    
    /// <summary>
    /// Jméno a příjmení zadavatele/zpracovatele
    /// </summary>
    /// <example>Jan Novák</example>
    public string? PerformerName { get; set; }
    
    /// <summary>
    /// Jméno typu procesu (nezkrácené)
    /// </summary>
    /// <example>Hlavní úvěrový proces</example>
    public string ProcessNameLong { get; set; } = null!;
    
    /// <summary>
    /// Dožádání: Příznak zaslání Dožádní přímo na klienta
    /// </summary>
    /// <example>false</example>
    public bool? SentToCustomer { get; set; }
    
    /// <summary>
    /// Dožádání: Číslo objednávky ocenění
    /// </summary>
    /// <example>123556</example>
    public int? OrderId { get; set; }

    /// <summary>
    /// Dožádání: Text požadavku a případné odpovědi (i včetně případných opakování komunikace). Řazeno chronologicky, nejstarší záznam je jako poslední.; Předání na specialistu: popis požadavku
    /// </summary>
    public List<TaskCommunicationItem> TaskCommunication { get; set; } = null!;

    public Amendments.Amendments Amendments { get; set; } = null!;
}