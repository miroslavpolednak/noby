namespace FOMS.Api.Endpoints.Household.Dto;

public class Household
{
    /// <summary>
    /// HouseholdId
    /// </summary>
    public int? Id { get; set; }
    
    /// <summary>
    /// ID typu domacnosti. Ciselnik HouseholdTypes
    /// </summary>
    /// <example>1</example>
    public int HouseholdTypeId { get; set; }

    /// <summary>
    /// Nazev typu domacnosti.
    /// </summary>
    public string? HouseholdTypeName { get; set; } = null!;
    
    /// <summary>
    /// Sekce Ostatni parametry
    /// </summary>
    public HouseholdData? Data { get; set; }
    
    /// <summary>
    /// Sekce Vydaje domacnosti
    /// </summary>
    public HouseholdExpenses? Expenses { get; set; }
    
    /// <summary>
    /// Klient navazani na domacnost
    /// </summary>
    public List<CustomerInHousehold>? Customers { get; set; }
}