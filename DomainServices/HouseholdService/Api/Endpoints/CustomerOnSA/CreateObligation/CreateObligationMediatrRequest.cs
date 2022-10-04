using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateObligation;

internal record CreateObligationMediatrRequest(CreateObligationRequest Request)
    : IRequest<CreateObligationResponse>, CIS.Core.Validation.IValidatableRequest
{
}
