namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

public sealed class GetMortgageRefixationResponse
    : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
{
    /// <summary>
    /// Seznam nabídek pro daný proces
    /// </summary>
    public List<Dto.Refinancing.RefinancingOfferDetail>? Offers { get; set; }
}
