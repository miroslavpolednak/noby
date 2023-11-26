using CIS.Infrastructure.StartupExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SharedComponents.DocumentDataStorage;

public static class DocumentDataStorageStartupExtensions
{
    public static WebApplicationBuilder AddDocumentDataStorage(this WebApplicationBuilder builder, string connectionStringKey = "default")
    {
        builder.AddEntityFramework<Database.DocumentDataDbContext>(connectionStringKey: connectionStringKey);

        builder.Services.AddScoped<IDocumentDataStorage>(provider =>
        {
            var dbContext = provider.GetRequiredService<Database.DocumentDataDbContext>();
            return new DocumentDataStorageProvider(dbContext, provider);
        });

        return builder;
    }
}
