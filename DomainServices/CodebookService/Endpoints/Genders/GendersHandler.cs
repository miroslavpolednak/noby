using DomainServices.CodebookService.Contracts.Endpoints.Genders;

namespace DomainServices.CodebookService.Endpoints.Genders;

public class GendersHandler
    : IRequestHandler<GendersRequest, List<Contracts.GenericCodebookItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<Contracts.GenericCodebookItem>> Handle(GendersRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new List<Contracts.GenericCodebookItem>
        {
            new Contracts.GenericCodebookItem() { Id = 1, Name = "Muž" },
            new Contracts.GenericCodebookItem() { Id = 2, Name = "Žena" }
        };
    }
}
