namespace DomainServices.CaseService.Api.Dto;

internal sealed class UpdateActiveTasksMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public Contracts.UpdateTaskItem[] Tasks { get; init; }
    
    public UpdateActiveTasksMediatrRequest(Contracts.UpdateActiveTasksRequest request)
    {
        CaseId = request.CaseId;
        Tasks = request.Tasks.ToArray();
    }
}
