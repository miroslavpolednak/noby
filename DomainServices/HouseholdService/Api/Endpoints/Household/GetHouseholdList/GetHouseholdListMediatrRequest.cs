using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.GetHouseholdList;

internal record GetHouseholdListMediatrRequest(int SalesArrangementId)
    : IRequest<GetHouseholdListResponse>
{
}