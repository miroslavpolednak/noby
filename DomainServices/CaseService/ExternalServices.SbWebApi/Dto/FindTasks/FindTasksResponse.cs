namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

public sealed class FindTasksResponse
{
    public required int ItemsFound { get; init; }

    /// <summary>
    /// Returns a collection of the found tasks, where each task is represented by a dictionary (key = mtdt_def, value = mtdt_val).
    /// </summary>
    public required  ICollection<IReadOnlyDictionary<string, string>> Tasks { get; init; }
}