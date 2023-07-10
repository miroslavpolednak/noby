﻿namespace NOBY.Dto.RealEstateValuation;

/// <summary>
/// Objekt je použit pouze pokud jde o Ocenění na case, který není v procesu (tedy pokud jde o HUBN nebo změnu).
/// </summary>
public sealed class LoanPurposeDetail
{
    /// <summary>
    /// ID účelu úvěru podle číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413109663">LoanPurpose (CIS_UCEL_UVERU_INT1)</a>
    /// </summary>
    /// <example>204</example>
    [Required]
    public IEnumerable<int> LoanPurposes { get; init; } = Enumerable.Empty<int>();
}