namespace NOBY.ApiContracts;

public partial class HouseholdCreateHouseholdRequest : IRequest<HouseholdInList>
{
    [JsonIgnore]
    public int SalesArrangementId { get; set; }

    /// <summary>
    /// Pouze pokud se vola handler z jineho handleru
    /// </summary>
    public bool HardCreate { get; set; }

    public HouseholdCreateHouseholdRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}