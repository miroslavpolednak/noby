using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record GetObligationListMediatrRequest(int CustomerOnSAId)
    : IRequest<GetObligationListResponse>
{
}
