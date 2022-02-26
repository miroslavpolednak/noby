using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record CreateCustomerMediatrRequest(CreateCustomerRequest Request)
    : IRequest<CreateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
}