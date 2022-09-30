using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record GetHouseholdListMediatrRequest(int SalesArrangementId)
    : IRequest<GetHouseholdListResponse>
{
}