namespace FOMS.Api.SharedHandlers.Requests;

internal sealed class SharedCreateSalesArrangementRequest
    : IRequest<int>
{
    public long CaseId { get; set; }
    public int SalesArrangementTypeId { get; set; }
    public int? OfferId { get; set; }
}
