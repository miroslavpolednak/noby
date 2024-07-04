namespace NOBY.ApiContracts;

public partial class OfferLinkMortgageExtraPaymentRequest
    : IRequest<OfferRefinancingLinkResult>
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    public OfferLinkMortgageExtraPaymentRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
