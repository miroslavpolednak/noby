namespace NOBY.ApiContracts;

public partial class OfferSimulateMortgageRetentionRequest
    : IRequest<OfferSimulateMortgageRetentionResponse>
{
    [JsonIgnore]
    public long CaseId { get; set; }

    public OfferSimulateMortgageRetentionRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
