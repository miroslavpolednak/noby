namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

public sealed class GetMortgageRefixationResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    /// <summary>
    /// Seznam nabídek pro daný proces
    /// </summary>
    public List<RefixationOfferDetail>? Offers { get; set; }
}

public sealed class RefixationOfferDetail
{
    public int OfferId { get; set; }

    /// <summary>
    /// Jedná se o zákonné sdělení
    /// </summary>
    public bool IsLegalNotice { get; set; }

    /// <summary>
    /// Jedná se o Aktuální nabídku
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Jedná se o sdělenou nabídku
    /// </summary>
    public bool IsCommunicated { get; set; }

    /// <summary>
    /// Jedná se o likovanou nabídku
    /// </summary>
    public bool IsLiked { get; set; }

    /// <summary>
    /// Jedná se o vybranou nabídku
    /// </summary>
    public bool IsSelected { get; set; }

    public int FixedRatePeriod { get; set; }
    
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
}