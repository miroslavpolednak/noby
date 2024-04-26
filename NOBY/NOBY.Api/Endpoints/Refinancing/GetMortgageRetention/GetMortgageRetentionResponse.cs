namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;

public sealed class GetMortgageRetentionResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    /// <summary>
    /// Výše poplatku
    /// </summary>
    public decimal FeeAmount { get; set; }

    /// <summary>
    /// Upravená výše poplatku
    /// </summary>
    public decimal? FeeAmountDiscounted { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby od.
    /// </summary>
    public DateTime InterestRateValidFrom { get; set; }

    /// <summary>
    /// Úroková sazba
    /// </summary>
    /// <example>4.7</example>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.2</example>
    public decimal? InterestRateDiscount { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>4.5</example>
    public decimal? InterestRateDiscounted { get => InterestRateDiscount.HasValue ? InterestRate - InterestRateDiscount : null; }

    /// <summary>
    /// Výše měsíční splátky.
    /// </summary>
    public decimal LoanPaymentAmount { get; set; }

    /// <summary>
    /// Výše měsíční splátky se zohledněním IC.
    /// </summary>
    public decimal? LoanPaymentAmountDiscounted { get; set; }

    /// <summary>
    /// Dokumentu refinancování
    /// </summary>
    public Dto.Refinancing.RefinancingDocument? Document { get; set; }

    public int? SignatureTypeDetailId { get; set; }

    /// <summary>
    /// Označuje zda má být aktivní button Generovat dokument
    /// </summary>
    public bool IsGenerateDocumentEnabled { get; set; }
}