using NOBY.Infrastructure.Configuration;
using NOBY.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace NOBY.Api.StartupExtensions;

public static class NobyAuthentication
{
    public static AuthenticationBuilder AddNobyAuthentication(this WebApplicationBuilder builder, AppConfiguration configuration)
    {
        // its mandatory to have auth scheme
        if (string.IsNullOrEmpty(configuration.Security?.AuthenticationScheme))
            throw new ArgumentException($"Authentication scheme is not specified. Please add correct NOBY.AuthenticationScheme in appsettings.json");

        // set up data protection
        var connectionString = builder.Configuration.GetConnectionString("dataProtection");
        if (!string.IsNullOrEmpty(connectionString))
        {
            builder.Services.AddDbContext<DataProtectionKeysContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDataProtection()
                .SetApplicationName("NobyFeApi")
                .PersistKeysToDbContext<DataProtectionKeysContext>()
                .SetDefaultKeyLifetime(TimeSpan.FromDays(100));
        }

        // set up auth provider
        switch (configuration.Security.AuthenticationScheme)
        {
            case AuthenticationConstants.CaasAuthScheme:
                return builder.Services.AddFomsCaasAuthentication();

            // fake authentication
            case AuthenticationConstants.MockAuthScheme:
                return builder.Services.AddFomsMockAuthentication();

            // simple login
            case AuthenticationConstants.SimpleLoginAuthScheme:
                return builder.Services.AddFomsSimpleLoginAuthentication(configuration.Security);

            // not existing auth scheme
            default:
                throw new NotImplementedException($"Authentication scheme '{configuration.Security.AuthenticationScheme}' not implemented");
        }
    }
}
