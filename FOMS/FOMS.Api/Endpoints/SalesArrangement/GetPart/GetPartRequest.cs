namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

internal class GetPartRequest
    : IRequest<object>
{
    public int SalesArrangementId { get; init; }
    public int PartId { get; init; }

    public GetPartRequest(int salesArrangementId, int partId)
    {
        SalesArrangementId = salesArrangementId;
        PartId = partId;
    }
}
