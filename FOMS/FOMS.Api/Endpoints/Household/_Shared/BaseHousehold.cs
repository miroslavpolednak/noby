namespace FOMS.Api.Endpoints.Household.Dto;

public abstract class BaseHousehold
{
    /// <summary>
    /// Sekce Ostatni parametry
    /// </summary>
    public HouseholdData? Data { get; set; }
    
    /// <summary>
    /// Sekce Vydaje domacnosti
    /// </summary>
    public HouseholdExpenses? Expenses { get; set; }
    
    /// <summary>
    /// Klient 1= sef domacnosti
    /// </summary>
    public CustomerInHousehold? Customer1 { get; set; }

    /// <summary>
    /// Klient 2= spoludluznik
    /// </summary>
    public CustomerInHousehold? Customer2 { get; set; }
}