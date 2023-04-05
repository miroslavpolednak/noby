using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Api.Mappers;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1;

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

        builder.Services.AddCisGrpcInfrastructure(typeof(Program), ErrorCodeMapper.Init());

        builder.AddExternalService<ISdfClient>();

        builder.AddExternalService<IDocumentServiceRepository>();

        builder.Services.AddSingleton<IDocumentMapper, DocumentMapper>();

        // databases
        builder.Services
            .AddDapper<IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("default")!);

        // dbcontext
        builder.AddEntityFramework<Database.DocumentArchiveDbContext>(connectionStringKey: "default");

        return builder;
    }
}