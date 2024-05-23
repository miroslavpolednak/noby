using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

public sealed class SimulateMortgageExtraPaymentRequest
    : IRequest<SimulateMortgageExtraPaymentResponse>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    /// <summary>
    /// Sleva z pokuty
    /// </summary>
    public decimal? FeeAmountDiscount { get; set; }

    /// <summary>
    /// Datum mimořádné splátky
    /// </summary>
    [Required]
    public DateOnly ExtraPaymentDate { get; set; }

    /// <summary>
    /// Částka mimořádné splátky
    /// </summary>
    public decimal? ExtraPaymentAmount { get; set;}

    /// <summary>
    /// Důvod mimořádné splátky
    /// </summary>
    [Required]
    public int ExtraPaymentReasonId { get; set; }

    /// <summary>
    /// Typ mimořádné splátky - částečná false, úplná true
    /// </summary>
    public bool IsExtraPaymentFullyRepaid { get; set; }

    internal SimulateMortgageExtraPaymentRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
