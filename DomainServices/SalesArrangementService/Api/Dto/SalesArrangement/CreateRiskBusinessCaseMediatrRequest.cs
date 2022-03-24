using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal record CreateRiskBusinessCaseMediatrRequest(int SalesArrangementId)
    : IRequest<CreateRiskBusinessCaseResponse>, CIS.Core.Validation.IValidatableRequest
{
}
