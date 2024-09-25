using SharedAudit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

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
                    logger.Log(AuditEventTypes.Noby001, "Pokus o přihlášení uživatele");

                    context.ProtocolMessage.State = SecurityHelpers.GetSafeRedirectUri(context.HttpContext.Request, _configuration);
                    return Task.CompletedTask;
                }
                else // vsechny standardni requesty
                {
                    var url = context.ProtocolMessage.CreateAuthenticationRequestUrl();
                    throw new CisAuthenticationException(url);
                }
            },
            OnAccessDenied = context =>
            {
                createLogger(context.HttpContext)?.OpenIdError("OnAccessDenied", context.AccessDeniedPath);

                context.Response.Redirect($"{_configuration.FailedSignInRedirectPath}?reason=authentication_access_denied");
                context.HandleResponse();

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                createLogger(context.HttpContext)?.OpenIdAuthenticationFailed(context.Exception);
                
                context.Response.Redirect($"{_configuration.FailedSignInRedirectPath}?reason=authentication_failed");
                context.HandleResponse();
                
                return Task.CompletedTask;
            },
            OnRemoteFailure = context =>
            {
                createLogger(context.HttpContext)?.OpenIdRemoteFailure(context.Failure);
                
                context.Response.Redirect($"{_configuration.FailedSignInRedirectPath}?reason=authentication_remote_failure");
                context.HandleResponse();

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                context.Properties!.RedirectUri = context.ProtocolMessage.State;
                context.HttpContext.Items.Add("noby_redirect_uri", context.ProtocolMessage.State);
                return Task.CompletedTask;
            },
            OnTicketReceived = context =>
            {
                // ziskat expiraci refresh tokenu pro ulozeni do claims
                var refreshToken = context.Properties!.GetTokenValue("refresh_token");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(refreshToken);
                context.HttpContext.Items.Add("noby_refreshtoken_exp", token.ValidTo);

                return Task.CompletedTask;
            }
        };
    }

    public void Configure(OpenIdConnectOptions options)
    {
        Configure(Options.DefaultName, options);
    }

    public const string CallbackPath = "/oidc-signin";

    private static ILogger? createLogger(HttpContext? context)
    {
        if (context is null)
        {
            return null;
        }

        return context.RequestServices.GetRequiredService<ILogger<CaasOpendIdHandler>>();
    }

    private readonly Configuration.AppConfigurationSecurity _configuration;

    public CaasOpendIdHandler(Configuration.AppConfiguration configuration)
    {
        _configuration = configuration.Security!;
    }
}
