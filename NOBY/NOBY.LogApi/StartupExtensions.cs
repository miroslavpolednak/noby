using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NOBY.Infrastructure.Configuration;
using NOBY.Infrastructure.Security;
using System.Reflection;

namespace NOBY.LogApi;

internal static class StartupExtensions
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");

    public static void AddLogApiSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "NOBY LOG API", Version = "v1" });

            x.EnableAnnotations();
            x.DescribeAllParametersInCamelCase();

            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
        });
    }

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
