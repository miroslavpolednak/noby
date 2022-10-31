namespace FOMS.Api.Endpoints.Household.Dto;

/// <summary>
/// Sekce Ostatni parametry
/// </summary>
public class HouseholdData
{
    /// <summary>
    /// Počet dětí 0-10 let
    /// </summary>
    /// <example>2</example>
    public int? ChildrenUpToTenYearsCount { get; set; }
    
    /// <summary>
    /// Počet dětí nad 10 let
    /// </summary>
    public int? ChildrenOverTenYearsCount { get; set; }
    
    /// <summary>
    /// Vypořádání majetku. Ciselnik ???
    /// </summary>
    public int? PropertySettlementId { get; set; }

    public bool? AreBothPartnersDeptors { get; set; }
}