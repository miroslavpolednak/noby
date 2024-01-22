using CIS.Infrastructure.Data;
using CIS.Infrastructure.StartupExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SharedComponents.Storage;

public interface ICisStorageServicesBuilder
{
    ICisStorageServicesBuilder AddStorageClient<TStorage>();

    ICisStorageServicesBuilder AddTempStorage();
}

internal sealed class CisStorageServicesBuilder
    : ICisStorageServicesBuilder
{
    private readonly WebApplicationBuilder _builder;

    public CisStorageServicesBuilder(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public ICisStorageServicesBuilder AddStorageClient<TStorage>()
    {
        _builder.Services.AddSingleton(typeof(IStorageClient<TStorage>), services =>
        {
            string storageName = typeof(TStorage).Name;
            var configuration = services.GetRequiredService<IOptions<Configuration.StorageConfiguration>>();

            if (configuration?.Value?.StorageClients?.TryGetValue(storageName, out Configuration.StorageClientConfiguration? clientConfiguration) ?? false)
            {
                return clientConfiguration.StorageType switch
                {
                    Configuration.StorageClientTypes.FileSystem => new StorageClients.FileSystemStorageClient<TStorage>(clientConfiguration!),
                    Configuration.StorageClientTypes.AzureBlob => new StorageClients.AzureBlobStorageClient<TStorage>(clientConfiguration!),
                    Configuration.StorageClientTypes.AmazonS3 => new StorageClients.AmazonS3StorageClient<TStorage>(clientConfiguration!),
                    _ => throw new CisConfigurationException(0, $"CisStorageServices: configuration type not found for client '{storageName}'")
                };
            }
            else
            {
                throw new CisConfigurationNotFound($"{Constants.StorageConfigurationKey}:{Constants.StorageClientsConfigurationKey}:{storageName}");
            }
        });

        return this;
    }

    public ICisStorageServicesBuilder AddTempStorage()
    {
        _builder.Services.AddSingleton<ITempStorage, TempStorage>();

        _builder.Services.AddDapper(services =>
        {
            var configuration = services.GetRequiredService<IOptions<Configuration.StorageConfiguration>>();

            string connectionString = (string.IsNullOrEmpty(configuration?.Value.TempStorage?.ConnectionString) 
                ? _builder.Configuration.GetConnectionString(CIS.Core.CisGlobalConstants.DefaultConnectionStringKey) 
                : configuration.Value.TempStorage?.ConnectionString)
                ?? throw new CisConfigurationNotFound($"{Constants.StorageConfigurationKey}:{Constants.TempStorageConfigurationKey}:ConnectionString");

            return new SqlConnectionProvider<Database.ITempStorageConnection>(connectionString);
        });

        return this;
    }
}
