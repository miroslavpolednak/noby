using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementStates
{
    public class SalesArrangementStatesHandler
        : IRequestHandler<SalesArrangementStatesRequest, List<SalesArrangementStateItem>>
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<SalesArrangementStateItem>> Handle(SalesArrangementStatesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new()
            {
                new SalesArrangementStateItem() { Id = 1, Name = "Rozpracováno", IsDefaultNewState = true },
                new SalesArrangementStateItem() { Id = 2, Name = "Předáno", IsDefaultNewState = false }
            };
        }
    }
}
