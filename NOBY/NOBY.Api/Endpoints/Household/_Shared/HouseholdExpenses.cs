namespace NOBY.Api.Endpoints.Household.Dto;

/// <summary>
/// Sekce Vydaje
/// </summary>
public class HouseholdExpenses
{
    /// <summary>
    /// sporeni
    /// </summary>
    /// <example>2500</example>
    public int? SavingExpenseAmount { get; set; }
    
    /// <summary>
    /// pojisteni
    /// </summary>
    /// <example>453</example>
    public int? InsuranceExpenseAmount { get; set; }
    
    /// <summary>
    /// naklady_na_bydleni
    /// </summary>
    public int? HousingExpenseAmount { get; set; }
    
    /// <summary>
    /// ostatni_vydaje
    /// </summary>
    /// <example>3300</example>
    public int? OtherExpenseAmount { get; set; }
}