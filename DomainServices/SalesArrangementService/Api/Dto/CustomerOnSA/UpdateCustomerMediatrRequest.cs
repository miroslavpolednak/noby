using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record UpdateCustomerMediatrRequest(UpdateCustomerRequest Request)
    : IRequest<UpdateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
}