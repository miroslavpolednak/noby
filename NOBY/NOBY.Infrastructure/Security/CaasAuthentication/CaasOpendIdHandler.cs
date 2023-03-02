using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace NOBY.Infrastructure.Security.CaasAuthentication;

internal sealed class CaasOpendIdHandler
     : IConfigureNamedOptions<OpenIdConnectOptions>
{
    public const string CallbackPath = "/oidc-signin";

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
    }

    public void Configure(OpenIdConnectOptions options)
    {
        Configure(Options.DefaultName, options);
    }
}
