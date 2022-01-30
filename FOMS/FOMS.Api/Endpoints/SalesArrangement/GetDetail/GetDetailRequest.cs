namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

internal class GetDetailRequest
    : IRequest<DomainServices.SalesArrangementService.Contracts.SalesArrangement>
{
    public int SalesArrangementId { get; set; }

    public GetDetailRequest(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
    }
}
