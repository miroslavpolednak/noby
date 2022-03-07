namespace FOMS.Api.Endpoints.Household.GetHousehold;

public class GetHouseholdResponse
    : Dto.BaseHousehold
{
    /// <summary>
    /// ID domacnosti
    /// </summary>
    public int HouseholdId { get; set; }
}
