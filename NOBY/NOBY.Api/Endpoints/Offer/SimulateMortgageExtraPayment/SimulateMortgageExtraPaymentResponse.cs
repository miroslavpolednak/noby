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
    public Dto.Refinancing.ExtraPaymentSimulationResult? SimulationResults { get; set; }
}
