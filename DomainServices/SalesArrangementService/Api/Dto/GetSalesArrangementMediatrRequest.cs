using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class GetSalesArrangementMediatrRequest
    : IRequest<GetSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public GetSalesArrangementMediatrRequest(SalesArrangementIdRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
    }
}
