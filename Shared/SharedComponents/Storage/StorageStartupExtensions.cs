using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedComponents.Storage.Configuration;

namespace SharedComponents.Storage;

public static class StorageStartupExtensions
{
    public static ICisStorageServicesBuilder AddCisStorageServices(this WebApplicationBuilder builder)
    {
        // add configuration
        builder.Services
            .AddOptions<Configuration.StorageConfiguration>()
            .Bind(builder.Configuration.GetSection(Constants.StorageConfigurationKey))
            .Validate(config =>
            {
                var type = config?.TempStorage?.StorageClient?.StorageType ?? StorageClientTypes.None;
                return type == StorageClientTypes.None ? true : validateStorageClient(config!.TempStorage!.StorageClient);
            }, "SharedComponents.Storage: TempStorage client provider specific configuration is not valid or is missing")
            .Validate(config =>
            {   
                return config?.StorageClients?.All(t => validateStorageClient(t.Value)) ?? true;
            }, "SharedComponents.Storage: some of StorageClients provider specific configuration is not valid or is missing")
            .ValidateOnStart();

        return new CisStorageServicesBuilder(builder);
    }

    private static bool validateStorageClient(StorageClientConfiguration config)
    {
        return config.StorageType switch
        {
            StorageClientTypes.FileSystem => !string.IsNullOrEmpty(config.FileSystem?.BasePath),
            StorageClientTypes.AzureBlob => !string.IsNullOrEmpty(config.AzureBlob?.ConnectionString),
            StorageClientTypes.AmazonS3 => !string.IsNullOrEmpty(config.AmazonS3?.ServiceUrl)
                && !string.IsNullOrEmpty(config.AmazonS3?.AccessKey)
                && !string.IsNullOrEmpty(config.AmazonS3?.SecretKey)
                && !string.IsNullOrEmpty(config.AmazonS3?.Bucket),
            _ => true
        };
    }
}