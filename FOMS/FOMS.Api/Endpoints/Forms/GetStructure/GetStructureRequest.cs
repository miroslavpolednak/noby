namespace FOMS.Api.Endpoints.Forms.Dto;

internal sealed class GetStructureRequest
    : IRequest<GetStructureResponse>
{
    public int SalesArrangementType { get; set; }
}
