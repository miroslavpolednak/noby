using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NOBY.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    private const string _configurationSectionName = "NOBY";

    public static AppConfiguration AddNobyConfig(this WebApplicationBuilder builder)
    {
        var appConfiguration = builder.Configuration
            .GetSection(_configurationSectionName)
            .Get<AppConfiguration>()
            ?? throw new CisConfigurationNotFound(_configurationSectionName);

        // register to DI
        builder.Services.AddSingleton(appConfiguration);

        return appConfiguration;
    }
}
