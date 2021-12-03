using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Dto.SalesArrangement;

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
