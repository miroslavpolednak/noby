﻿namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;

public sealed class GetMortgageRetentionResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    public string? Comment { get; set; }

    /// <summary>
    /// Seznam odpovědních kódů
    /// </summary>
    public List<Dto.Refinancing.RefinancingResponseCode>? ResponseCodes { get; set; }

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
    public decimal? InterestRateDiscounted => InterestRateDiscount.HasValue && InterestRateDiscount != 0 ? InterestRate - InterestRateDiscount : null;

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
    public Dto.Refinancing.RefinancingDocument Document { get; set; } = null!;
}