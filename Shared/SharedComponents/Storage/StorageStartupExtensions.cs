using CIS.Core;
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

    public static WebApplicationBuilder AddCisStorageServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<Configuration.StorageConfiguration>()
            .Bind(builder.Configuration.GetSection(_configurationStorageKey));

        // temp storage
        builder.Services.AddScoped<ITempStorage, TempStorage>();

        builder.Services.AddDapper(services =>
        {
            var configuration = services.GetRequiredService<IOptions<Configuration.StorageConfiguration>>();
            
            string connectionString = (string.IsNullOrEmpty(configuration?.Value.TempStorage?.ConnectionString) ? builder.Configuration.GetConnectionString(CisGlobalConstants.DefaultConnectionStringKey) : configuration.Value.TempStorage?.ConnectionString)
                ?? throw new CisConfigurationNotFound($"{_configurationStorageKey}:TempStorage:ConnectionString");

            return new SqlConnectionProvider<ITempStorageConnection>(connectionString);
        });

        return builder;
    }
}
