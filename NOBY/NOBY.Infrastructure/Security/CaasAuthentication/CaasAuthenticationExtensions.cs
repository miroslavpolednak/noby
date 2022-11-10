using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace NOBY.Infrastructure.Security;

public static class CaasAuthenticationExtensions
{
    public static AuthenticationBuilder AddFomsCaasAuthentication(this IServiceCollection services)
    {
        return services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "mvccode";

                options.Events.OnSigningOut = async e =>
                {
                    // revoke refresh token on sign-out
                    //await e.HttpContext.RevokeUserRefreshTokenAsync();
                };
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Configuration = new OpenIdConnectConfiguration
                {
                    IntrospectionEndpoint = "https://dev1-caas.kb.cz/openam/oauth2/introspect",
                    RegistrationEndpoint = "https://dev1-caas.kb.cz/openam/oauth2/register",
                    AuthorizationEndpoint = "https://dev1-caasauth.kb.cz/autfe/ssologin",
                    TokenEndpoint = "https://dev1-caas.kb.cz/openam/oauth2/access_token",
                    UserInfoEndpoint = "https://dev1-caas.kb.cz/openam/oauth2/userinfo"
                };
            });
    }
}
