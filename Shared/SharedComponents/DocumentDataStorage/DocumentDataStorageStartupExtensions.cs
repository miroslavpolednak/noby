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

        // zaregistrovat mappery
        builder.Services.Scan(scan => scan
            .FromEntryAssembly()
            .AddClasses(classes => classes.AssignableTo(typeof(IDocumentDataMapper<,>)))
            .AsImplementedInterfaces());

        return builder;
    }
}
