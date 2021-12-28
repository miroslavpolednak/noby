namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

internal class SavePartRequest
    : IRequest
{
    public int SalesArrangementId { get; init; }
    public int PartId { get; init; }
    public object PartData { get; init; }

    public SavePartRequest(int salesArrangementId, int partId, object partData)
    {
        PartData = partData;
        SalesArrangementId = salesArrangementId;
        PartId = partId;
    }
}
