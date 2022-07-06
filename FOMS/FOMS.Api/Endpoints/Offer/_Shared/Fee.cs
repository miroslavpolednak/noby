namespace FOMS.Api.Endpoints.Offer.Dto;

public class Fee
{
	public int FeeId { get; set; }
	public decimal DiscountPercentage { get; set; }
	public decimal? TariffSum { get; set; }
	public decimal? ComposedSum { get; set; }
	public decimal? FinalSum { get; set; }
	public int? MarketingActionId { get; set; }
	public string? Name { get; set; }
	public string? ShortNameForExample { get; set; }
	public string? TariffName { get; set; }
	public string? UsageText { get; set; }
	public string? TariffTextWithAmount { get; set; }
	public string? CodeKB { get; set; }
	public bool DisplayAsFreeOfCharge { get; set; }
	public bool IncludeInRPSN { get; set; }
	public string? Periodicity { get; set; }
	public DateTime? AccountDateFrom { get; set; }
}
