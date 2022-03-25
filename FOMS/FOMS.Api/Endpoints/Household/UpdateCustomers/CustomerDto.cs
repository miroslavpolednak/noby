namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

public class CustomerDto 
    : Dto.BaseCustomer, Services.CreateCustomer.IClientInfo
{
    /// <summary>
    /// Identita klienta, pokud se ma zalozit novy CustomerOnSA
    /// </summary>
    public CIS.Foms.Types.CustomerIdentity? Identity { get; set; }
}