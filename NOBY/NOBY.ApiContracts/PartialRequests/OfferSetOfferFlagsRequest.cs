namespace NOBY.ApiContracts;

public partial class OfferSetOfferFlagsRequest
    : IRequest
{
    [JsonIgnore]
    public int OfferId { get; private set; }

    public OfferSetOfferFlagsRequest InfuseId(int offerId)
    {
        this.OfferId = offerId;
        return this;
    }
}
