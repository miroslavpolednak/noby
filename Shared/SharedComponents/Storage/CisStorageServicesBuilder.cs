using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SharedComponents.Storage;

public interface ICisStorageServicesBuilder
{
    ICisStorageServicesBuilder AddStorageClient<TStorage>() where TStorage : IStorageClient;
}

internal sealed class CisStorageServicesBuilder
    : ICisStorageServicesBuilder
{
    private readonly IServiceCollection _services;

    public CisStorageServicesBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ICisStorageServicesBuilder AddStorageClient<TStorage>()
        where TStorage : IStorageClient
    {
        _services.AddSingleton(typeof(TStorage), services =>
        {
            string storageName = typeof(TStorage).Name;
            var configuration = services.GetRequiredService<IOptions<Configuration.StorageConfiguration>>();

            if (configuration?.Value?.StorageClients?.TryGetValue(storageName, out Configuration.StorageClientConfiguration? clientConfiguration) ?? false)
            {
                return clientConfiguration.StorageType switch
                {
                    Configuration.StorageClientTypes.FileSystem => new StorageClients.FileSystemStorageClient(clientConfiguration!),
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
}
