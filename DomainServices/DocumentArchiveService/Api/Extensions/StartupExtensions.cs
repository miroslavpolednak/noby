using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.DocumentArchiveService.Api.Mappers;
using ExternalServices.Sdf;
using ExternalServices.Sdf.V1.Clients;
using ExternalServicesTcp;
using ExternalServicesTcp.V1.Repositories;
using FluentValidation;

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

    public static WebApplicationBuilder AddDocumentArchiveService(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddCisGrpcInfrastructure(typeof(Program));

        var appConfig = configuration.GetSection(AppConfiguration.SectionName).Get<AppConfiguration>();

        builder.AddExternalService<ISdfClient>();

        builder.AddExternalService<IDocumentServiceRepository>();

        builder.Services.AddSingleton<IDocumentMapper, DocumentMapper>();

        // databases
        builder.Services
            .AddDapper<Data.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("default"));

        return builder;
    }
}