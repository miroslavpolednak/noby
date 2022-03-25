using FOMS.Api.Endpoints.Household.Dto;
using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

public class UpdateHouseholdRequest
    : IRequest
{
    [JsonIgnore]
    internal int HouseholdId;

    /// <summary>
    /// Sekce Ostatni parametry
    /// </summary>
    public HouseholdData? Data { get; set; }

    /// <summary>
    /// Sekce Vydaje domacnosti
    /// </summary>
    public HouseholdExpenses? Expenses { get; set; }

    internal UpdateHouseholdRequest InfuseId(int householdId)
    {
        this.HouseholdId = householdId;
        return this;
    }
}
