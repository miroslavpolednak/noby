using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.DocumentArchiveService.Api.Extensions;
using DomainServices.DocumentArchiveService.Api.ExternalServices.Sdf.V1;
using DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.V1;
using DomainServices.DocumentArchiveService.Api.Mappers;

namespace DomainServices.DocumentArchiveService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration?.ServiceUser2LoginBinding is null)
            throw new CisConfigurationNotFound("AppConfiguration");
    }

    public static WebApplicationBuilder AddDocumentArchiveService(this WebApplicationBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddCisGrpcInfrastructure(typeof(Program));

        builder.AddExternalService<ISdfClient>();

        builder.AddExternalService<IDocumentServiceRepository>();

        builder.Services.AddSingleton<IDocumentMapper, DocumentMapper>();

        // databases
        builder.Services
            .AddDapper<Data.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("default"));

        // dbcontext
        builder.AddEntityFramework<Database.DocumentArchiveDbContext>(connectionStringKey: "default");

        return builder;
    }
}