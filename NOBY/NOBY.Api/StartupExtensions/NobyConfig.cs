using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.StartupExtensions;

internal static class NobyConfig
{
    private const string _configurationSectionName = "NOBY";

    public static AppConfiguration AddNobyConfig(this WebApplicationBuilder builder)
    {
        AppConfiguration appConfiguration = new();

        // bind from config files
        builder.Configuration.Bind(_configurationSectionName, appConfiguration);
        // register to DI
        builder.Services.AddSingleton(appConfiguration);

        return appConfiguration;
    }
}
