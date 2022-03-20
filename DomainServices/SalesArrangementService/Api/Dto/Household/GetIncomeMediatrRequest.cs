using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record GetIncomeMediatrRequest(int IncomeId)
    : IRequest<Income>
{
}
