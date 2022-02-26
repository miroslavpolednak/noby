using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record CreateHouseholdMediatrRequest(CreateHouseholdRequest Request)
    : IRequest<CreateHouseholdResponse>, CIS.Core.Validation.IValidatableRequest
{
}