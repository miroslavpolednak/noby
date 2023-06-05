namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangements;

internal sealed record GetSalesArrangementsRequest(long CaseId)
    : IRequest<List<Dto.SalesArrangementListItem>>
{
}