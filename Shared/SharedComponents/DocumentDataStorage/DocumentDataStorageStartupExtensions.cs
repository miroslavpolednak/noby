using CIS.Infrastructure.StartupExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedComponents.DocumentDataStorage.Database;

namespace SharedComponents.DocumentDataStorage;

public static class DocumentDataStorageStartupExtensions
{
    public static WebApplicationBuilder AddDocumentDataStorage(this WebApplicationBuilder builder, string connectionStringKey = "default")
    {
        builder.Services.AddDapper<IDocumentDataStorageConnection>(builder.Configuration.GetConnectionString(connectionStringKey)!);

        builder.Services.AddScoped<IDocumentDataStorage, DocumentDataStorageProvider>();

        return builder;
    }
}
