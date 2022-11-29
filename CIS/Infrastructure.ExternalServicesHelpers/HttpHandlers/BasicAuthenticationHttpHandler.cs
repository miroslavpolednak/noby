using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers;

/// <summary>
/// Middleware pridavajici Authorization header do requestu. Username a password je zadavan do konstruktoru handleru pri pridavani HttpClienta.
/// </summary>
public sealed class BasicAuthenticationHttpHandler
    : DelegatingHandler
{
    private readonly AuthenticationHeaderValue _headerValue;

    public BasicAuthenticationHttpHandler(AuthenticationHeaderValue headerValue)
    {
        _headerValue = headerValue;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = _headerValue;

        return await base.SendAsync(request, cancellationToken);
    }

    public static AuthenticationHeaderValue PrepareAuthorizationHeaderValue(IExternalServiceBasicAuthenticationConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.Username) || string.IsNullOrEmpty(configuration.Password))
            throw new ArgumentNullException("Username or Password for Basic Authentication has not been set");

        var bytes = Encoding.ASCII.GetBytes($"{configuration.Username}:{configuration.Password}");
        return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
    }
}
