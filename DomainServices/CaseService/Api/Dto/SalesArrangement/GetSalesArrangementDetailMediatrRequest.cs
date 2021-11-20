using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Dto.SalesArrangement;

internal sealed class GetSalesArrangementDetailMediatrRequest
    : IRequest<GetSalesArrangementDetailResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public GetSalesArrangementDetailMediatrRequest(SalesArrangementIdRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
    }
}
