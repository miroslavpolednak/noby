using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record CreateObligationMediatrRequest(CreateObligationRequest Request)
    : IRequest<CreateObligationResponse>, CIS.Core.Validation.IValidatableRequest
{
}
