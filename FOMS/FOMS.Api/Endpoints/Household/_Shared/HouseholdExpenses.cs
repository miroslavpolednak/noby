namespace FOMS.Api.Endpoints.Household.Dto;

/// <summary>
/// Sekce Vydaje
/// </summary>
public class HouseholdExpenses
{
    /// <summary>
    /// sporeni
    /// </summary>
    public int? SavingExpenseAmount { get; set; }
    
    /// <summary>
    /// pojisteni
    /// </summary>
    public int? InsuranceExpenseAmount { get; set; }
    
    /// <summary>
    /// naklady_na_bydleni
    /// </summary>
    public int? HousingExpenseAmount { get; set; }
    
    /// <summary>
    /// ostatni_vydaje
    /// </summary>
    public int? OtherExpenseAmount { get; set; }
}