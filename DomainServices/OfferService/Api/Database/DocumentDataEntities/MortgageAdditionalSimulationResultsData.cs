namespace DomainServices.OfferService.Api.Database.DocumentDataEntities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal sealed class MortgageAdditionalSimulationResultsData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public List<PaymentScheduleData>? PaymentScheduleSimple { get; set; }
	public List<MarketingActionData>? MarketingActions { get; set; }
	public List<FeeData>? Fees { get; set; }

    public sealed class PaymentScheduleData
    {
        public int PaymentIndex { get; set; }
        public string PaymentNumber { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }
    }

    public sealed class MarketingActionData
    {
        public string Code { get; set; }
        public int Requested { get; set; }
        public int? Applied { get; set; }
        public int? MarketingActionId { get; set; }
        public decimal? Deviation { get; set; }
	    public string Name { get; set; }
    }

    public sealed class FeeData
    {
        public int FeeId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal? TariffSum { get; set; }
        public decimal? ComposedSum { get; set; }
        public decimal? FinalSum { get; set; }
        public int? MarketingActionId { get; set; }
	    public string Name { get; set; }
	    public string ShortNameForExample { get; set; }
	    public string TariffName { get; set; }
	    public string UsageText  { get; set; }
	    public string TariffTextWithAmount  { get; set; }
	    public string CodeKB  { get; set; }
	    public bool DisplayAsFreeOfCharge  { get; set; }
	    public bool IncludeInRPSN  { get; set; }
	    public string Periodicity  { get; set; }
        public DateTime? AccountDateFrom { get; set; }
    }
}
