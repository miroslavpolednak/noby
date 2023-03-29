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
        var login = _serviceProvider.GetService<CIS.Core.Security.ICurrentUserAccessor>()?.User?.Login;

        if (!string.IsNullOrEmpty(login))
        {
            int index = login.IndexOf('=');
            if (index > 0)
            {
                request.Headers.Add("X-KB-Party-Identity-In-Service", $$"""{"partyIdIS":[{"partyId":{"id":"{{login[..index]}}","idScheme":"{{login[(index + 1)..]}}"},"usg":"AUTH"}]}""");
            }
        }
        
        //TODO zamockovano https://jira.kb.cz/browse/HFICH-4442, https://jira.kb.cz/browse/HFICH-366
        //request.Headers.Add("X-KB-Party-Identity-In-Service", $$$"""{"partyIdIS":[{"partyId":{"id":"A09FK3","idScheme":{"code":"KBUID"}},"usg":"BA"},{"partyId":{"id":"JMARKOVA","idScheme":{"code":"KBAD"}},"usg":"BA"}]}""");

        return await base.SendAsync(request, cancellationToken);
    }
}
