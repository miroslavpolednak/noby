using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NOBY.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    public static AppConfiguration AddNobyConfig(this WebApplicationBuilder builder)
    {
        var appConfiguration = builder.Configuration
            .GetSection(CIS.Core.CisGlobalConstants.DefaultAppConfigurationSectionName)
            .Get<AppConfiguration>()
            ?? throw new CisConfigurationNotFound(CIS.Core.CisGlobalConstants.DefaultAppConfigurationSectionName);

        // register to DI
        builder.Services.AddSingleton(appConfiguration);

        return appConfiguration;
    }
}
