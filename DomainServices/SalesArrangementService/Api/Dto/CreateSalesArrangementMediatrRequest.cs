using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class CreateSalesArrangementMediatrRequest
    : IRequest<CreateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{
    public CreateSalesArrangementRequest Request { get; init; }
    
    public CreateSalesArrangementMediatrRequest(CreateSalesArrangementRequest request)
    {
        Request = request;
    }
}
