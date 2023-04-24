namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

public sealed class FindByTaskIdRequest
{
    public required int TaskIdSb { get; init; }

    public ICollection<int>? TaskStates { get; init; }
}