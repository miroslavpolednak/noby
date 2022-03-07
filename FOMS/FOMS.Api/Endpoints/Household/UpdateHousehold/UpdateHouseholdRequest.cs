using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

public class UpdateHouseholdRequest
    : Dto.BaseHousehold, IRequest<UpdateHouseholdResponse>
{
    [JsonIgnore]
    internal int HouseholdId;

    internal UpdateHouseholdRequest InfuseId(int householdId)
    {
        this.HouseholdId = householdId;
        return this;
    }
}
