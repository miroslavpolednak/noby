namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

internal class GetPartRequest
    : IRequest<object>
{
    public int SalesArrangementId { get; set; }

    public GetPartRequest(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
    }
}
