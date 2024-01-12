using CIS.Infrastructure.Data;
using CIS.Infrastructure.StartupExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedComponents.Storage.Database;

namespace SharedComponents.Storage;

public static class StorageStartupExtensions
{
    private const string _configurationStorageKey = "CisStorage";

    public static WebApplicationBuilder AddStorageServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<Configuration.StorageConfiguration>()
            .Bind(builder.Configuration.GetSection(_configurationStorageKey));

        // temp storage
        builder.Services.AddScoped<ITempStorage, TempStorage>();

        builder.Services.AddDapper(services =>
        {
            var configuration = services.GetRequiredService<IOptions<Configuration.StorageConfiguration>>();

            if (string.IsNullOrEmpty(configuration?.Value.TempStorage?.ConnectionString))
            {
                throw new CisConfigurationNotFound(_configurationStorageKey);
            }

            return new SqlConnectionProvider<ITempStorageConnection>(configuration.Value.TempStorage!.ConnectionString);
        });

        return builder;
    }
}
