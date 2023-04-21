namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

public sealed class FindByCaseIdRequest 
{
    public required long CaseId { get; init; }

    public required string SearchPattern { get; init; }

    public ICollection<int>? TaskStates { get; init; }
}