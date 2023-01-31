using Microsoft.AspNetCore.Http;

namespace CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers;

/// <summary>
/// Middleware přidávájící KB hlavičku s informací o přihlášeném uživateli do requestu.
/// </summary>
/// <remarks>
/// Přidává hlavičku X-KB-Party-Identity-In-Service.
/// </remarks>
public sealed class KbPartyHeaderHttpHandler
    : DelegatingHandler
{
    // vim ze to neni hezke, ale nenapada me jak jinak
    private readonly IServiceProvider _serviceProvider;

    public KbPartyHeaderHttpHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var userId = _serviceProvider.GetService<CIS.Core.Security.ICurrentUserAccessor>()?.User?.Id;

        request.Headers.Add("X-KB-Party-Identity-In-Service", $$"""{"partyIdIS":[{"partyId":{"id":"{{userId}}","idScheme":"MPAD"},"usg":"AUTH"}]}""");

        return await base.SendAsync(request, cancellationToken);
    }
}
