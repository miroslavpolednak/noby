namespace FOMS.Api.Endpoints.Household.GetHouseholds;

public class GetHouseholdsResponse
{
    /// <summary>
    /// Seznam domacnosti pro dany SalesArrangement
    /// </summary>
    public List<Dto.HouseholdInList>? Households { get; set; }
}
