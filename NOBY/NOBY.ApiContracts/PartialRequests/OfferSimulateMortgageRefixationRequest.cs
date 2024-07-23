namespace NOBY.ApiContracts;

public partial class OfferSimulateMortgageRefixationRequest
        : IRequest<List<OfferSimulateMortgageRefixationResponse>>
{
    [JsonIgnore]
    public long CaseId { get; set; }

    public OfferSimulateMortgageRefixationRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
