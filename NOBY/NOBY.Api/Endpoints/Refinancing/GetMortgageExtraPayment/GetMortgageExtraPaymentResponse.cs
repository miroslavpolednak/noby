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

    /// <summary>
    /// Způsob předání dokumentů
    /// </summary>
    public HandoverObject? Handover { get; set; }

    public decimal ExtraPaymentAmount { get; set; }

    public decimal PrincipalAmount { get; set; }

    public DateTime ExtraPaymentDate { get; set; }

    public bool IsExtraPaymentFullyRepaid { get; set; }

    /// <summary>
    /// Upravená výše poplatku
    /// </summary>
    public decimal? FeeAmountDiscount { get; set; }

    public NOBY.Dto.Refinancing.ExtraPaymentSimulationResult? SimulationResults { get; set; }

    public sealed class HandoverObject
    {
        public int HandoverTypeDetailId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
