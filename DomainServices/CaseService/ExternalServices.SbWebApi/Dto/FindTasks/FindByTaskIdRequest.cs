namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

public sealed class FindByTaskIdRequest
{
    public required int TaskSbId { get; init; }

    public ICollection<int>? TaskStates { get; init; }
}