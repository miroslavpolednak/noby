using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record GetIncomeListMediatrRequest(int CustomerOnSAId)
    : IRequest<GetIncomeListResponse>
{
}
