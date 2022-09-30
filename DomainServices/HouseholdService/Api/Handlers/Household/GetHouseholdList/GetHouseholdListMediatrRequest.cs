using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Handlers.Household.GetHouseholdList;

internal record GetHouseholdListMediatrRequest(int SalesArrangementId)
    : IRequest<GetHouseholdListResponse>
{
}