using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

public sealed class SimulateMortgageExtraPaymentRequest
    : IRequest<SimulateMortgageExtraPaymentResponse>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    public DateTime ExtraPaymentDate { get; set; }

    public decimal ExtraPaymentAmount { get; set;}

    public string ExtraPaymentReason { get; set; }

    public string ExtraPaymentType { get; set; }

    internal SimulateMortgageExtraPaymentRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
