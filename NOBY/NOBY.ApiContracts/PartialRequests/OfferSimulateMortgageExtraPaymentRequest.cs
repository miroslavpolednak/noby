namespace NOBY.ApiContracts;

public partial class OfferSimulateMortgageExtraPaymentRequest
    : IRequest<OfferSimulateMortgageExtraPaymentResponse>
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    public OfferSimulateMortgageExtraPaymentRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
