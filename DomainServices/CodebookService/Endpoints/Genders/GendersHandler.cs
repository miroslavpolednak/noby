using DomainServices.CodebookService.Contracts.Endpoints.Genders;

namespace DomainServices.CodebookService.Endpoints.Genders;

public class GendersHandler
    : IRequestHandler<GendersRequest, List<GenderItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<GenderItem>> Handle(GendersRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new List<GenderItem>
        {
            new GenderItem() { Id = 1, Name = "Muž", RDMCode = "M" },
            new GenderItem() { Id = 2, Name = "Žena", RDMCode ="F" }
        };
    }
}
