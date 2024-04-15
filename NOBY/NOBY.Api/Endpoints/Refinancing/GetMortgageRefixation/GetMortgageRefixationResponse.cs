namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

public sealed class GetMortgageRefixationResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    /// <summary>
    /// Seznam nabídek pro daný proces
    /// </summary>
    public List<Dto.Refinancing.RefinancingOfferDetail>? Offers { get; set; }

    /// <summary>
    /// Datum platnosti sdělených nabídek
    /// </summary>
    public DateTime? CommunicatedOffersValidTo { get; set; }

    /// <summary>
    /// Datum odeslání zákonných oznámení
    /// </summary>
    public DateTime? LegalNoticeGeneratedDate { get; set; }
}
