namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.UpdateTask;
public class UpdateTaskRequest
{
    public required int TaskIdSb { get; init; }

    public Dictionary<string, string> Metadata { get; set; } = null!;
}
