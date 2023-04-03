namespace NOBY.Api.Endpoints.SalesArrangement.GetList;

internal sealed record GetListRequest(long CaseId)
    : IRequest<List<Dto.SalesArrangementListItem>>
{
}