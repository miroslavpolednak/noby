using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal class CreateCustomerMediatrRequest
    : IRequest<CreateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
    public CreateCustomerRequest Request { get; init; }
    
    public CreateCustomerMediatrRequest(CreateCustomerRequest request)
    {
        Request = request;
    }
}