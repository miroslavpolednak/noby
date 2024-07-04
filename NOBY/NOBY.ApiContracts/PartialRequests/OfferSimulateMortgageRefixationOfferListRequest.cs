namespace NOBY.ApiContracts;

public partial class OfferSimulateMortgageRefixationOfferListRequest
    : IRequest<OfferSimulateMortgageRefixationOfferListResponse>
{
    [JsonIgnore]
    public long CaseId { get; set; }

    public OfferSimulateMortgageRefixationOfferListRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
