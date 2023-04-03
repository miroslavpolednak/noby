namespace NOBY.Api.Endpoints.Offer.Dto;

public sealed class MarketingActionItem
{
	public string Code { get; set; } = string.Empty;

	public bool Requested { get; set; }

	public bool Applied { get; set; }

	public int? MarketingActionId { get; set; }

	public decimal? Deviation { get; set; }

	public string Name { get; set; } = string.Empty;
}
