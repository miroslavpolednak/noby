using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.StartupExtensions;

internal static class NobyConfig
{
    public static AppConfiguration AddNobyConfig(this WebApplicationBuilder builder)
    {
        AppConfiguration appConfiguration = new();

        // bind from config files
        builder.Configuration.Bind("NOBY", appConfiguration);
        // register to DI
        builder.Services.AddSingleton(appConfiguration);

        return appConfiguration;
    }
}
