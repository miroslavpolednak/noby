using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SetOfferFlags;

public sealed class SetOfferFlagsRequest
    : IRequest
{
    [JsonIgnore]
    internal int OfferId { get; private set; }

    /// <summary>
    /// Seznam příznaku k nastavení
    /// </summary>
    public List<SetOfferFlagsRequestItem>? Flags { get; set; }

    internal SetOfferFlagsRequest InfuseId(int offerId)
    {
        this.OfferId = offerId;
        return this;
    }
}

public sealed class SetOfferFlagsRequestItem
{
    /// <summary>
    /// Typ příznaku
    /// </summary>
    public OfferFlagTypes FlagType { get; set; }

    /// <summary>
    /// True = set flag, False = unset flag
    /// </summary>
    public bool SetFlag { get; set; }
}