using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SharedDto.LinkModelation;

public abstract class LinkMortgageBaseRequest<TLinkMortgageRequest> : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    [JsonIgnore]
    internal int SalesArrangementId { get; set; }

    public int OfferId { get; set; }

    internal TLinkMortgageRequest InfuseId(long caseId, int salesArrangementId)
    {
        CaseId = caseId;
        SalesArrangementId = salesArrangementId;

        return GetRequestInstance();
    }

    protected abstract TLinkMortgageRequest GetRequestInstance();
}