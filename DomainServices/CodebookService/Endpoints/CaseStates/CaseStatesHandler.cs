using DomainServices.CodebookService.Contracts.Endpoints.CaseStates;

namespace DomainServices.CodebookService.Endpoints.CaseStates;

public class CaseStatesHandler
    : IRequestHandler<CaseStatesRequest, List<CaseStateItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<CaseStateItem>> Handle(CaseStatesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new List<CaseStateItem>
        {
            new CaseStateItem() { Id = 1, Name = "novy", IsDefaultNewState = true },
            new CaseStateItem() { Id = 2, Name = "ve zpracovani", IsDefaultNewState = false }
        };
    }
}
