namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

public class CustomerDto 
    : Dto.BaseCustomer
{
    /// <summary>
    /// Příznak zamknutí příjmů daného CustomerOnSA
    /// </summary>
    public bool LockedIncome { get; set; }

    /// <summary>
    /// Identita klienta, pokud se ma zalozit novy CustomerOnSA
    /// </summary>
    public CIS.Foms.Types.CustomerIdentity? Identity { get; set; }
}