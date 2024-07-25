namespace NOBY.ApiContracts;

public partial class HouseholdUpdateCustomersRequest : IRequest<HouseholdUpdateCustomersResponse>
{
    [JsonIgnore]
    public int HouseholdId;

    public HouseholdUpdateCustomersRequest InfuseId(int householdId)
    {
        this.HouseholdId = householdId;
        return this;
    }  
}