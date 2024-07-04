namespace NOBY.ApiContracts;

public partial class OfferLinkMortgageOfferRequest
    : IRequest<OfferRefinancingLinkResult>
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public int SalesArrangementId { get; private set; }

    public OfferLinkMortgageOfferRequest InfuseId(long caseId, int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        this.CaseId = caseId;
        return this;
    }
}
