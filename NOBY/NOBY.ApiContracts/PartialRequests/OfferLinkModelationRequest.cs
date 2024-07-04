namespace NOBY.ApiContracts;

public partial class OfferLinkModelationRequest
    : IRequest
{
    [JsonIgnore]
    public int SalesArrangementId { get; private set; }

    public OfferLinkModelationRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}
