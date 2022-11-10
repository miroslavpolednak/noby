using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

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

    internal UpdateCustomersRequest InfuseId(int householdId)
    {
        this.HouseholdId = householdId;
        return this;
    }    
}
