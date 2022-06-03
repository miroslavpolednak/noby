namespace DomainServices.CaseService.Api.Dto;

internal record GetTaskListMediatrRequest(long CaseId)
    : IRequest<Contracts.GetTaskListResponse>, CIS.Core.Validation.IValidatableRequest
{

}
