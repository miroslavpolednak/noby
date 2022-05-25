using DomainServices.CodebookService.Contracts.Endpoints.IdentitySchemes;

namespace DomainServices.CodebookService.Endpoints.IdentitySchemes;

public class IdentitySchemesHandler
    : IRequestHandler<IdentitySchemesRequest, List<IdentitySchemeItem>>
{
    public Task<List<IdentitySchemeItem>> Handle(IdentitySchemesRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<IdentitySchemeItem>
        {
            new IdentitySchemeItem() { Id = 1, Code = "KBID", Name = "Identifikátor KB klienta v Customer managementu", MandantId = 2, Category = "Customer", ChannelId = null },
            new IdentitySchemeItem() { Id = 2, Code = "MPSBID", Name = "PartnerId ve Starbuildu", MandantId = 1, Category = "Customer", ChannelId = null },
            new IdentitySchemeItem() { Id = 3, Code = "MPEKID", Name = "KlientId v eKmenu", MandantId = 1, Category = "Customer", ChannelId = null },
            new IdentitySchemeItem() { Id = 4, Code = "KBUID", Name = null, MandantId = 2, Category = "User", ChannelId = 4 },
            new IdentitySchemeItem() { Id = 5, Code = "M04ID", Name = null, MandantId = 1, Category = "User", ChannelId = 1 },
            new IdentitySchemeItem() { Id = 6, Code = "M17ID", Name = null, MandantId = 1, Category = "User", ChannelId = 1 },
            new IdentitySchemeItem() { Id = 7, Code = "BrokerId", Name = null, MandantId = 2, Category = "User", ChannelId = 6 },
            new IdentitySchemeItem() { Id = 8, Code = "MPAD", Name = null, MandantId = 1, Category = "User", ChannelId = 1 },
        });
    }

    private readonly ILogger<IdentitySchemesHandler> _logger;

    public IdentitySchemesHandler(
        ILogger<IdentitySchemesHandler> logger)
    {
        _logger = logger;
    }
}