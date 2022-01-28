using DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses;

namespace DomainServices.CodebookService.Endpoints.MaritalStatuses;

public class MaritalStatusesHandler
    : IRequestHandler<MaritalStatusesRequest, List<MaritalStatusItem>>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<MaritalStatusItem>> Handle(MaritalStatusesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new()
        {
            new MaritalStatusItem() { Id = 1, Name = "Svobodný(á)", C4mStatus = "S" },
            new MaritalStatusItem() { Id = 2, Name = "Ženatý/Vdaná", C4mStatus = "M" },
            new MaritalStatusItem() { Id = 3, Name = "Rozvedený(á)", C4mStatus = "D" },
            new MaritalStatusItem() { Id = 4, Name = "Vdoved/vdova", C4mStatus = "W" },
            new MaritalStatusItem() { Id = 5, Name = "Registrovaný/á partner/ka", C4mStatus = "R" }
        };
    }
}