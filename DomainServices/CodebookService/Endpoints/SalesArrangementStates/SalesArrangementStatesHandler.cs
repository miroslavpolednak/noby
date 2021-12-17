using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementStates
{
    public class ProductInstanceStatesHandler
        : IRequestHandler<SalesArrangementStatesRequest, List<SalesArrangementStateItem>>
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<SalesArrangementStateItem>> Handle(SalesArrangementStatesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new List<SalesArrangementStateItem>
            {
                new SalesArrangementStateItem() { Id = 0, Name = "Draft", Code = "" },
                new SalesArrangementStateItem() { Id = 1, Name = "Nový", Code = "NEW", IsDefaultNewState = true },
                new SalesArrangementStateItem() { Id = 2, Name = "Rozpracováno", Code = "IN_PROGRESS" },
                new SalesArrangementStateItem() { Id = 3, Name = "Schváleno", Code = "APPROVED" },
                new SalesArrangementStateItem() { Id = 4, Name = "Odesláno", Code = "SENT" },
                new SalesArrangementStateItem() { Id = 5, Name = "Podepsáno", Code = "SIGNED" },
                new SalesArrangementStateItem() { Id = 6, Name = "Zkontrolováno", Code = "CHECKED" },
                new SalesArrangementStateItem() { Id = 7, Name = "Zrušeno", Code = "CANCELED" }
            };
        }
    }
}
