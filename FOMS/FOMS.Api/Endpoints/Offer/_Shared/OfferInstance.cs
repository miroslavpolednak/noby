namespace FOMS.Api.Endpoints.Offer.Dto;

internal class OfferInstance
{
    public int OfferInstanceId { get; set; }
    public int ProductInstanceTypeId { get; set; }
    public string? ResourceProcessId { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
}