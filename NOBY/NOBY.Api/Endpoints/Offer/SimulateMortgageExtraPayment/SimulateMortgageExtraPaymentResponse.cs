namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

public sealed class SimulateMortgageExtraPaymentResponse
{
    /// <summary>
    /// ID vytvorene simulace.
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Původní simulace nalinkovaná na SA
    /// </summary>
    public Dto.Refinancing.ExtraPaymentSimulationResult? OldOffer { get; set; }

    /// <summary>
    /// Nově nasimolovaná nabídka
    /// </summary>
    public Dto.Refinancing.ExtraPaymentSimulationResult NewOffer { get; set; } = null!;
}
