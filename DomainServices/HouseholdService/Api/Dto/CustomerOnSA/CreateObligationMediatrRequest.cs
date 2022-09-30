using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record CreateObligationMediatrRequest(CreateObligationRequest Request)
    : IRequest<CreateObligationResponse>, CIS.Core.Validation.IValidatableRequest
{
}
