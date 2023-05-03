namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

public sealed class FindByContractNumberRequest
{
    public required string ContractNumber { get; init; }

    public required string SearchPattern { get; init; }

    public ICollection<int>? TaskStates { get; init; }
}