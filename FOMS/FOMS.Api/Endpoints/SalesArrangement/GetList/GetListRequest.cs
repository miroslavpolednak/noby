using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.SalesArrangement.GetList;

internal record GetListRequest(long CaseId)
    : IRequest<List<Dto.SalesArrangementListItem>>, IValidatableRequest
{
}