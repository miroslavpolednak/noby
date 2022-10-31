namespace FOMS.Api.Endpoints.Household.GetHousehold;

public class GetHouseholdResponse
{
    /// <summary>
    /// ID domacnosti
    /// </summary>
    public int HouseholdId { get; set; }

    public bool AreCustomersPartners { get; set; }

    /// <summary>
    /// Sekce Ostatni parametry
    /// </summary>
    public Dto.HouseholdData? Data { get; set; }

    /// <summary>
    /// Sekce Vydaje domacnosti
    /// </summary>
    public Dto.HouseholdExpenses? Expenses { get; set; }

    /// <summary>
    /// Klient 1= sef domacnosti
    /// </summary>
    public CustomerInHousehold? Customer1 { get; set; }

    /// <summary>
    /// Klient 2= spoludluznik
    /// </summary>
    public CustomerInHousehold? Customer2 { get; set; }
}
