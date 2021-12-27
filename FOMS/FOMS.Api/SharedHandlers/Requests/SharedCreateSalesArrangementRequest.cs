namespace FOMS.Api.SharedHandlers.Requests;

internal sealed class SharedCreateSalesArrangementRequest
    : IRequest<int>
{
    public long CaseId { get; set; }
    public int ProductInstanceType { get; set; }
    public int? OfferInstanceId { get; set; }
}
