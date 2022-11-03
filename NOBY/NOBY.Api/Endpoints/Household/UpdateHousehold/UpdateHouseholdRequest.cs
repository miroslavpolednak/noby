using System.Text.Json.Serialization;
using NOBY.Api.Endpoints.Household.Dto;

namespace NOBY.Api.Endpoints.Household.UpdateHousehold;

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
