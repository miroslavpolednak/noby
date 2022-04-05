using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

public class UpdateCustomersRequest
    : IRequest<UpdateCustomersResponse>
{
    [JsonIgnore]
    internal int HouseholdId;

    /// <summary>
    /// ID hlavniho frajera v domacnosti
    /// </summary>
    public CustomerDto? Customer1 { get; set; }

    /// <summary>
    /// ID spoludluznika
    /// </summary>
    public CustomerDto? Customer2 { get; set; }

    /// <summary>
    /// Zavazky obou klientu vytazene do objektu Customer1 a Customer2.
    /// </summary>
    public List<Dto.HouseholdCustomerObligation>? Obligations { get; set; }

    internal UpdateCustomersRequest InfuseId(int householdId)
    {
        this.HouseholdId = householdId;
        return this;
    }    
}
