using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

internal record GetListRequest(long CaseId)
    : IRequest<List<SalesArrangementListItem>>, IValidatableRequest
{
}