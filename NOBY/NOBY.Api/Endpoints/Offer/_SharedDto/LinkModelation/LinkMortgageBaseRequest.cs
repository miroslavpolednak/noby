using NOBY.Dto.Refinancing;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SharedDto.LinkModelation;

public abstract class LinkMortgageBaseRequest<TLinkMortgageRequest> : IRequest<RefinancingLinkResult>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    public int OfferId { get; set; }

    internal TLinkMortgageRequest InfuseId(long caseId)
    {
        CaseId = caseId;

        return GetRequestInstance();
    }

    protected abstract TLinkMortgageRequest GetRequestInstance();
}