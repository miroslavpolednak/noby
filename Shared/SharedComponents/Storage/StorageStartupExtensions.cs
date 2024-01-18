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
    public static ICisStorageServicesBuilder AddCisStorageServices(this WebApplicationBuilder builder)
    {
        // add configuration
        builder.Services
            .AddOptions<Configuration.StorageConfiguration>()
            .Bind(builder.Configuration.GetSection(Constants.StorageConfigurationKey));

        // temp storage
        addTempStorage(builder);

        return new CisStorageServicesBuilder(builder.Services);
    }

    private static void addTempStorage(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITempStorage, TempStorage>();

        builder.Services.AddDapper(services =>
        {
            var configuration = services.GetRequiredService<IOptions<Configuration.StorageConfiguration>>();

            string connectionString = (string.IsNullOrEmpty(configuration?.Value.TempStorage?.ConnectionString) ? builder.Configuration.GetConnectionString(CisGlobalConstants.DefaultConnectionStringKey) : configuration.Value.TempStorage?.ConnectionString)
                ?? throw new CisConfigurationNotFound($"{Constants.StorageConfigurationKey}:{Constants.TempStorageConfigurationKey}:ConnectionString");

            return new SqlConnectionProvider<ITempStorageConnection>(connectionString);
        });
    }
}