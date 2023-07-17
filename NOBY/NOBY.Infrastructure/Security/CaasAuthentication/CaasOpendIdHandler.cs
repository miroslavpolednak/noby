using CIS.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace NOBY.Infrastructure.Security.CaasAuthentication;

internal sealed class CaasOpendIdHandler
     : IConfigureNamedOptions<OpenIdConnectOptions>
{
    public void Configure(string? name, OpenIdConnectOptions options)
    {
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("nobylogin");
        options.Scope.Add("offline_access");

        options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.CallbackPath = CallbackPath;
        options.ResponseMode = "query";
        options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.SaveTokens = true;

        options.ClientId = _configuration.ClientId;
        options.ClientSecret = _configuration.ClientSecret;
        options.Authority = _configuration.Authority;

        // pouzij pro komunikaci sys proxy
        if (_configuration.UseSystemProxy)
        {
            options.BackchannelHttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; },
                UseProxy = true,
                Proxy = null, // use system proxy
                DefaultProxyCredentials = System.Net.CredentialCache.DefaultNetworkCredentials
            };
        }

        options.Events = new OpenIdConnectEvents
        {
            OnRedirectToIdentityProvider = context =>
            {
                // autentizacni requesty
                if (context.HttpContext.Request.Path.StartsWithSegments(AuthenticationConstants.DefaultAuthenticationUrlSegment))
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<IAuditLogger>();
                    logger.Log(CIS.Infrastructure.Telemetry.AuditLog.AuditEventTypes.Noby001, "Pokus o přihlášení uživatele");

                    context.ProtocolMessage.State = getRedirectUri(context.HttpContext.Request);
                    return Task.CompletedTask;
                }
                else // vsechny standardni requesty
                {
                    var url = context.ProtocolMessage.CreateAuthenticationRequestUrl();
                    throw new CIS.Core.Exceptions.CisAuthenticationException(url);
                }
            },
            OnTokenValidated = context =>
            {
                context.Properties!.RedirectUri = context.ProtocolMessage.State;
                context.HttpContext.Items.Add("noby_redirect_uri", context.ProtocolMessage.State);
                return Task.CompletedTask;
            }
        };
    }

    public void Configure(OpenIdConnectOptions options)
    {
        Configure(Options.DefaultName, options);
    }

    public const string CallbackPath = "/oidc-signin";

    /// <summary>
    /// Ziskej redirectUri z query stringu nebo vrat default.
    /// </summary>
    /// <returns>
    /// Kontroluje, zda uri v query stringu je validni a z teto domeny.
    /// </returns>
    private string getRedirectUri(HttpRequest request)
    {
        var redirectUri = request.Query[AuthenticationConstants.RedirectUriQueryParameter];
        if (!string.IsNullOrEmpty(redirectUri))
        {
            try
            {
                var safeUri = new Uri(redirectUri!);
                if (string.IsNullOrEmpty(safeUri.Host))
                {
                    return $"https://{request.Host}{redirectUri}";
                }
                else if (safeUri.Authority == request.Host.Value && safeUri.Scheme == "https")
                {
                    return safeUri.ToString();
                }
            }
            catch
            {
                // spatne zadane URI v query stringu. Zalogovat?
            }
        }
        return $"https://{request.Host}{_configuration.DefaultRedirectPathAfterSignIn}";
    }
    private readonly Configuration.AppConfigurationSecurity _configuration;

    public CaasOpendIdHandler(Configuration.AppConfiguration configuration)
    {
        _configuration = configuration.Security!;
    }
}
