using CIS.InternalServices.Storage.Api.BlobStorage;

namespace CIS.InternalServices.Storage.Api;

public static class BlobStartupExtension
{
    public static IServiceCollection AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var c = new BlobConfiguration();
        configuration.GetSection("Blob").Bind(c);

        // pridat konfiguraci do DI
        services.AddSingleton(c);

        // repo
        services.AddTransient<BlobRepository>();

        // pridat provider podle nastaveni configu
        switch (c.Provider)
        {
            case BlobConfiguration.Providers.LocalFilesystem:
                services.AddTransient<IBlobStorageProvider, BlobStorage.Providers.LocalFilesystemProvider>();
                break;
        }

        return services;
    }
}
