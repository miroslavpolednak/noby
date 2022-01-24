using DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses;

namespace DomainServices.CodebookService.Endpoints.MaritalStatuses;

public class MaritalStatusesHandler
    : IRequestHandler<MaritalStatusesRequest, List<Contracts.GenericCodebookItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<Contracts.GenericCodebookItem>> Handle(MaritalStatusesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new List<Contracts.GenericCodebookItem>
        {
            new Contracts.GenericCodebookItem() { Id = 1, Name = "Svobodný(á)", IsActual = true },
            new Contracts.GenericCodebookItem() { Id = 2, Name = "Ženatý/Vdaná", IsActual = true },
            new Contracts.GenericCodebookItem() { Id = 3, Name = "Rozvedený(á)", IsActual = true },
            new Contracts.GenericCodebookItem() { Id = 4, Name = "Vdoved/vdova", IsActual = true }
        };
    }
}