using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

public sealed class SimulateMortgageExtraPaymentRequest
    : IRequest<SimulateMortgageExtraPaymentResponse>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    /// <summary>
    /// Datum mimořádné splátky
    /// </summary>
    public DateTime ExtraPaymentDate { get; set; }

    /// <summary>
    /// Částka mimořádné splátky
    /// </summary>
    public decimal ExtraPaymentAmount { get; set;}

    /// <summary>
    /// Důvod mimořádné splátky
    /// </summary>
    public int ExtraPaymentReasonId { get; set; }

    /// <summary>
    /// Typ mimořádné splátky
    /// </summary>
    public int ExtraPaymentTypeId { get; set; }

    internal SimulateMortgageExtraPaymentRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
