using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementStates
{
    public class ProductInstanceStatesHandler
        : IRequestHandler<SalesArrangementStatesRequest, List<Contracts.GenericCodebookItem>>
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<Contracts.GenericCodebookItem>> Handle(SalesArrangementStatesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new List<Contracts.GenericCodebookItem>
            {
                new Contracts.GenericCodebookItem() { Id = 0, Name = "Rozpracováno" },
                new Contracts.GenericCodebookItem() { Id = 1, Name = "Předáno" }
            };
        }
    }
}
