using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class GetSalesArrangementDataMediatrRequest
    : IRequest<GetSalesArrangementDataResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public GetSalesArrangementDataMediatrRequest(SalesArrangementIdRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
    }
}
