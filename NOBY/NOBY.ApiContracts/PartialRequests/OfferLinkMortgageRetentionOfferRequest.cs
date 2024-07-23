namespace NOBY.ApiContracts;

public partial class OfferLinkMortgageRetentionOfferRequest
    : IRequest<OfferRefinancingLinkResult>
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    public OfferLinkMortgageRetentionOfferRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
