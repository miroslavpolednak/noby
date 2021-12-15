using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class CreateSalesArrangementMediatrRequest
    : IRequest<CreateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public int SalesArrangementType { get; init; }
    public long? ProductInstanceId { get;init; }

    public CreateSalesArrangementMediatrRequest(CreateSalesArrangementRequest request)
    {
        CaseId = request.CaseId;
        SalesArrangementType = request.SalesArrangementType;
        ProductInstanceId = request.ProductInstanceId;
    }
}
