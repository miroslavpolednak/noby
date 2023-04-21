namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.CancelTask;

public sealed class CancelTaskRequest
    : RequestBase
{
    public int TaskSBId { get; set; }
}
