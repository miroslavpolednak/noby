namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.CreateTask;

public sealed class CreateTaskRequest
{
    public int TaskTypeId { get; set; }
    public int ProcessId { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = null!;
}
