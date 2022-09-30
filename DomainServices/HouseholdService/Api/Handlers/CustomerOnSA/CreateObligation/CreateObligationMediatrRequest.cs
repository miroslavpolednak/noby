using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA.CreateObligation;

internal record CreateObligationMediatrRequest(CreateObligationRequest Request)
    : IRequest<CreateObligationResponse>, CIS.Core.Validation.IValidatableRequest
{
}
