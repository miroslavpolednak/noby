namespace NOBY.ApiContracts;

public partial class RefinancingUpdateMortgageRefixationRequest
    : IRequest<RefinancingSharedOfferLinkResult>
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    public RefinancingUpdateMortgageRefixationRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
