namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class CreateCaseResponse
{
    public long CaseId { get; set; }
    public int SalesArrangementId { get; set; }
    public int OfferId { get; set; }
}
