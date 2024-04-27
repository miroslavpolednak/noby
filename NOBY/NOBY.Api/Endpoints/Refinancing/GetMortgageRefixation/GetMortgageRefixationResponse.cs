using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

public sealed class GetMortgageRefixationResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    public string? Comment { get; set; }

    /// <summary>
    /// Seznam odpovědních kódů
    /// </summary>
    public List<RefinancingResponseCode>? ResponseCodes { get; set; }

    /// <summary>
    /// Seznam nabídek pro daný proces
    /// </summary>
    public List<Dto.Refinancing.RefinancingOfferDetail>? Offers { get; set; }

    /// <summary>
    /// Sleva sazby z IC
    /// </summary>
    public decimal? InterestRateDiscount { get; set; }

    /// <summary>
    /// Datum platnosti sdělených nabídek
    /// </summary>
    public DateTime? CommunicatedOffersValidTo { get; set; }

    /// <summary>
    /// Datum odeslání zákonných oznámení
    /// </summary>
    public DateTime? LegalNoticeGeneratedDate { get; set; }
}
