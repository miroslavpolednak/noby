namespace FOMS.Api.SharedHandlers.Requests;

internal sealed class SharedCreateSalesArrangementRequest
    : IRequest<int>
{
    public long CaseId { get; set; }
    public int ProductInstanceTypeId { get; set; }
    public int? OfferInstanceId { get; set; }
}
