namespace NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.Amendments;

/// <summary>
/// Tento objekt je relevantní pouze pro typ úkolu Dožádání
/// </summary>
public class AmendmentsRequest : Amendments
{
    /// <summary>
    /// Dožádání: Příznak zaslání Dožádní přímo na klienta
    /// </summary>
    /// <example>false</example>
    public bool? SentToCustomer { get; set; }
    
    /// <summary>
    /// Dožádání: Číslo objednávky ocenění (Atribut je relevantní jen pro dožádání k ocenění.)
    /// </summary>
    /// <example>123556</example>
    public int? OrderId { get; set; }
}