namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

public sealed class SimulateMortgageRetentionResponse
{
    /// <summary>
    /// ID vytvorene simulace.
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Výše měsíční splátky.
    /// </summary>
    public decimal LoanPaymentAmount { get; set; }

    /// <summary>
    /// Výše měsíční splátky se zohledněním IC.
    /// </summary>
    public decimal? LoanPaymentAmountIndividualPrice { get; set; }
}
