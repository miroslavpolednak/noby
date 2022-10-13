namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed record GetSalesArrangementListMediatrRequest(long CaseId)
    : IRequest<Contracts.GetSalesArrangementListResponse>, CIS.Core.Validation.IValidatableRequest
{
}
