namespace NOBY.Api.Endpoints.SalesArrangement.GetList;

internal record GetListRequest(long CaseId)
    : IRequest<List<Dto.SalesArrangementListItem>>
{
}