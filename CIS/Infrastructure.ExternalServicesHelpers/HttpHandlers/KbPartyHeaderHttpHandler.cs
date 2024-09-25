using CIS.Core;
using SharedTypes.Types;
using System.ComponentModel.DataAnnotations;

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

    // identita ktera se ma pouzit, pokud neni k dispozici kontext prihlaseneho uzivatele
    private readonly FallbackIdentity? _fallbackIdentity;

    public KbPartyHeaderHttpHandler(IServiceProvider serviceProvider, UserIdentity? fallbackIdentity = null)
    {
        _serviceProvider = serviceProvider;

        if (fallbackIdentity is not null)
        {
            _fallbackIdentity = new(fallbackIdentity.Scheme.GetAttribute<DisplayAttribute>()!.Name!, fallbackIdentity.Identity);
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        bool useFallback = true;
        var login = _serviceProvider.GetService<CIS.Core.Security.ICurrentUserAccessor>()?.User?.Login;

        if (!string.IsNullOrEmpty(login))
        {
            int index = login.IndexOf('=');
            if (index > 0)
            {
                request.Headers.Replace("X-KB-Party-Identity-In-Service", $$"""{"partyIdIS":[{"partyId":{"idScheme":"{{login[..index]}}","id":"{{login[(index + 1)..]}}"},"usg":"AUTH"}]}""");
                useFallback = false;
            }
        }

        if (useFallback && _fallbackIdentity is not null)
        {
			request.Headers.Replace("X-KB-Party-Identity-In-Service", $$"""{"partyIdIS":[{"partyId":{"idScheme":"{{_fallbackIdentity.Schema}}","id":"{{_fallbackIdentity.Id}}"},"usg":"AUTH"}]}""");
		}

        return await base.SendAsync(request, cancellationToken);
    }

    private sealed record FallbackIdentity(string Schema, string Id) { }
}
