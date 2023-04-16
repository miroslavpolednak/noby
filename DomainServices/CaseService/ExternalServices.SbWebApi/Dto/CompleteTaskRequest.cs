namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto;

public sealed class CompleteTaskRequest
{
    public required int TaskIdSb { get; init; }

    public string? TaskUserResponse { get; init; }

    public IEnumerable<string> TaskDocumentIds { get; init; } = Enumerable.Empty<string>();
}