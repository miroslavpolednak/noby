namespace NOBY.ApiContracts;

public partial class HouseholdUpdateHouseholdRequest : IRequest
{
    [JsonIgnore]
    public int HouseholdId;

    public HouseholdUpdateHouseholdRequest InfuseId(int householdId)
    {
        this.HouseholdId = householdId;
        return this;
    }
}