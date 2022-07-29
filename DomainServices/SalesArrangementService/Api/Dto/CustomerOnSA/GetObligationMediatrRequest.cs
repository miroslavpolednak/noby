using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record GetObligationMediatrRequest(int ObligationId)
    : IRequest<Obligation>
{
}
