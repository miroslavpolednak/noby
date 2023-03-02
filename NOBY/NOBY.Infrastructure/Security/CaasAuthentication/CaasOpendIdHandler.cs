using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
                if (context.HttpContext.Request.Path.StartsWithSegments(AuthenticationConstants.DefaultAuthenticationUrlSegment))
                {
                    return Task.CompletedTask;
                }
                else
                {
                    var url = context.ProtocolMessage.CreateAuthenticationRequestUrl();
                    throw new CIS.Core.Exceptions.CisAuthenticationException(url);
                }
            }
        };
    }

    public void Configure(OpenIdConnectOptions options)
    {
        Configure(Options.DefaultName, options);
    }

    public const string CallbackPath = "/oidc-signin";

    private readonly Configuration.AppConfigurationSecurity _configuration;

    public CaasOpendIdHandler(Configuration.AppConfiguration configuration)
    {
        _configuration = configuration.Security!;
    }
}
