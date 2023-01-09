using DomainServices.CodebookService.Contracts.Endpoints.MktActionCodesSavings;

namespace DomainServices.CodebookService.Endpoints.MktActionCodesSavings;

public class MktActionCodesSavingsHandler
    : IRequestHandler<MktActionCodesSavingsRequest, List<Contracts.GenericCodebookItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<Contracts.GenericCodebookItem>> Handle(MktActionCodesSavingsRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new List<Contracts.GenericCodebookItem>
    {
        new Contracts.GenericCodebookItem() { Id = 1, Name = "Efekt", IsValid = true },
        new Contracts.GenericCodebookItem() { Id = 2, Name = "Efekt Plus", IsValid = true }
    };
    }
}
