namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.CompleteTask;

public sealed class CompleteTaskRequest
{
    public required int TaskIdSb { get; init; }

    public Dictionary<string, string> Metadata { get; set; } = null!;
}