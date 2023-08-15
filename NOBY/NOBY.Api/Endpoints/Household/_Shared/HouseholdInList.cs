namespace NOBY.Api.Endpoints.Household.Dto;

/// <summary>
/// Domacnost v seznamu domacnosti - pro vytvoreni tab listu
/// </summary>
public class HouseholdInList
{
    /// <summary>
    /// ID domacnosti
    /// </summary>
    public int HouseholdId { get; set; }

    public int SalesArrangementId { get; set; }
    
    public long CaseId { get; set; }
    
    /// <summary>
    /// ID typu domacnosti
    /// </summary>
    public int HouseholdTypeId { get; set; }

    /// <summary>
    /// Typ domacnosti slovne
    /// </summary>
    public string? HouseholdTypeName { get; set; }
}
