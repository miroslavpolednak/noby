using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record GetHouseholdListMediatrRequest(int SalesArrangementId)
    : IRequest<GetHouseholdListResponse>
{
}