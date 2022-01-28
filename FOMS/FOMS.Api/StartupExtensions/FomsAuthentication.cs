using FOMS.Infrastructure.Security;

namespace FOMS.Api.StartupExtensions;

public static class FomsAuthentication
{
    public static WebApplicationBuilder AddFomsAuthentication(this WebApplicationBuilder builder, Infrastructure.Configuration.AppConfiguration configuration)
    {
        // its mandatory to have auth scheme
        if (string.IsNullOrEmpty(configuration.AuthenticationScheme))
            throw new ArgumentException($"Authentication scheme is not specified. Please add correct NOBY.AuthenticationScheme in appsettings.json");

        switch (configuration.AuthenticationScheme)
        {
            // fake authentication
            case AuthenticationConstants.MockAuthScheme:
                builder.Services
                    .AddAuthentication(AuthenticationConstants.MockAuthScheme)
                    .AddScheme<MockAuthSchemeOptions, MockAuthenticationHandler>(AuthenticationConstants.MockAuthScheme, options => { });
                break;

            // simple login
            case AuthenticationConstants.SimpleLoginAuthScheme:
                builder.Services
                    .AddAuthentication(AuthenticationConstants.SimpleLoginAuthScheme)
                    .AddCookie(config =>
                    {
                        config.Cookie.Name = "nobyauth";
                        config.Cookie.HttpOnly = true;
                        config.Cookie.Path = "/";
                        config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        config.Cookie.SameSite = SameSiteMode.Lax;
                    });
                break;

            // not existing auth scheme
            default:
                throw new NotImplementedException($"Authentication scheme '{configuration.AuthenticationScheme}' not implemented");
        }
        
        return builder;
    }
}
