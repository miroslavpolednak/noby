namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class GetSalesArrangementListMediatrRequest
    : IRequest<Contracts.GetSalesArrangementListResponse>, CIS.Core.Validation.IValidatableRequest
{
    public Contracts.GetSalesArrangementListRequest Request { get; init; }

    public GetSalesArrangementListMediatrRequest(Contracts.GetSalesArrangementListRequest request)
    {
        Request = request;
    }
}
