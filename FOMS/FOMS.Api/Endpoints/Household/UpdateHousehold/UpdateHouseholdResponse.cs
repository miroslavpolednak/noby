namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

public class UpdateHouseholdResponse
{
    /// <summary>
    /// ID updatovane domacnosti
    /// </summary>
    public int HouseholdId { get; set; }

    /// <summary>
    /// ID nove ulozeneho nebo updatovaneho customera 1
    /// </summary>
    public int? CustomerOnSAId1 { get; set; }

    /// <summary>
    /// ID nove ulozeneho nebo updatovaneho customera 2
    /// </summary>
    public int? CustomerOnSAId2 { get; set; }
}
