namespace FOMS.Api.Endpoints.Offer.Dto;

public sealed class MarketingActionInput
{
    public bool Domicile { get; set; }

	public bool HealthRiskInsurance { get; set; }

	public bool RealEstateInsurance { get; set; }

	public bool IncomeLoanRatioDiscount { get; set; }

	public bool UserVip { get; set; }
}
