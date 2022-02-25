using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record GetHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Household>
{
}