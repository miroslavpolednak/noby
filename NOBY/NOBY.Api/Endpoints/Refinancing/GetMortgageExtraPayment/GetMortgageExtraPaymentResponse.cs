namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;

public sealed class GetMortgageExtraPaymentResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    /// <summary>
    /// Dokumentu refinancování
    /// </summary>
    public Dto.Refinancing.RefinancingDocument? Document { get; set; }

    /// <summary>
    /// Souhlasy
    /// </summary>
    public List<Dto.Refinancing.RefinancingDocument>? Agreements { get; set; }

    public decimal ExtraPaymentAmount { get; set; }

    public decimal PrincipalAmount { get; set; }

    public DateTime? ExtraPaymentDate { get; set; }

    public bool IsExtraPaymentFullyRepaid { get; set; }

    public NOBY.Dto.Refinancing.BaseExtraPaymentSimulationResult? OfferResult { get; set; }
}
