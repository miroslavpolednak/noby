namespace NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.Amendments;

/// <summary>
/// Tento objekt je relevantní pouze pro některé typy Konzultací, a to sice pro Dotaz k ocenění a pro Vyhotovení ZOV.
/// </summary>
public class AmendmentsConsultationData
{
    /// <summary>
    /// Konzultace: Číslo objednávky ocenění (Atribut je relevantní jen pro konzultaci Dotaz k ocenění a pro konzultaci Vyhotovení ZOV.)
    /// </summary>
    /// <example>123556</example>
    public int? OrderId { get; set; }
}