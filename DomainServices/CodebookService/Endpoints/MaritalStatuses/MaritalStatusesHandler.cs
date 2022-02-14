using DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses;

namespace DomainServices.CodebookService.Endpoints.MaritalStatuses;

public class MaritalStatusesHandler
    : IRequestHandler<MaritalStatusesRequest, List<MaritalStatusItem>>
{
    public Task<List<MaritalStatusItem>> Handle(MaritalStatusesRequest request, CancellationToken cancellationToken)
    {
        var model = new List<MaritalStatusItem>()
        {
            new MaritalStatusItem() { Id = 0, Name = "neuveden", IsValid = true },
            new MaritalStatusItem() { Id = 1, Name = "svobodný(á)", RdmMaritalStatusCode = "S", IsValid = true },
            new MaritalStatusItem() { Id = 2, Name = "ženatý/Vdaná", RdmMaritalStatusCode = "M", IsValid = true },
            new MaritalStatusItem() { Id = 3, Name = "rozvedený(á)", RdmMaritalStatusCode = "D", IsValid = true },
            new MaritalStatusItem() { Id = 4, Name = "vdoved/vdova", RdmMaritalStatusCode = "W", IsValid = true },
            new MaritalStatusItem() { Id = 4, Name = "druh/družka", RdmMaritalStatusCode = "S", IsValid = true },
            new MaritalStatusItem() { Id = 5, Name = "registrovaný/á partner/ka", RdmMaritalStatusCode = "R", IsValid = true }
        };
        return Task.FromResult(model);
    }
}