using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record CreateIncomeMediatrRequest(CreateIncomeRequest Request)
    : IRequest<CreateIncomeResponse>, CIS.Core.Validation.IValidatableRequest
{
}
