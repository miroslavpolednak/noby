namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;

public sealed class GetMortgageExtraPaymentResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    /// <summary>
    /// Dokumentu refinancování
    /// </summary>
    public Dto.Refinancing.RefinancingDocument Document { get; set; } = null!;

    /// <summary>
    /// Souhlasy
    /// </summary>
    public List<AgreementDocument>? Agreements { get; set; }

    /// <summary>
    /// Způsob předání dokumentů
    /// </summary>
    public HandoverObject? Handover { get; set; }

    public decimal ExtraPaymentAmount { get; set; }

	public decimal ExtraPaymentAmountIncludingFee { get; set; }

	public decimal PrincipalAmount { get; set; }

    public DateOnly ExtraPaymentDate { get; set; }

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

    public sealed class AgreementDocument
    {
        /// <summary>
        /// eArchiv ID dokumentu
        /// </summary>
        public string DocumentId { get; set; } = string.Empty;

        /// <summary>
        /// EA kód dokumentu
        /// </summary>
        public int DocumentEACode { get; set; }
    }
}
