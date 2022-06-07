namespace DomainServices.CaseService.Api.Dto;

internal record class UpdateActiveTasksMediatrRequest(long CaseId, Contracts.UpdateTaskItem[] Tasks)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    
}
