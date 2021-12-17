namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

internal class GetDetailRequest
    : IRequest<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse>
{
    public int SalesArrangementId { get; set; }

    public GetDetailRequest(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
    }
}
