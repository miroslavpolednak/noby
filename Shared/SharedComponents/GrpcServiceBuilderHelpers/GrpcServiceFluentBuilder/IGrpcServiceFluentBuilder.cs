using CIS.Core.Configuration;
using CIS.Core.ErrorCodes;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;

namespace SharedComponents.GrpcServiceBuilderHelpers;

public interface IGrpcServiceFluentBuilder
{
    /// <summary>
    /// Skipne z registrace funkcnost pro ziskani kontextoveho uzivatele sluzby. Je potreba kdyz dana sluzba nepouziva UserService.
    /// </summary>
    /// <remarks>Odstranuje z pipeline zaroven Audit logging.</remarks>
    IGrpcServiceFluentBuilder SkipServiceUserContext();

    /// <summary>
    /// Možnost změny nastavení gRCP služeb. Supluje .AddGrpc(o)
    /// </summary>
    IGrpcServiceFluentBuilder AddGrpcServiceOptions(Action<GrpcServiceOptions> changeOptions);

    /// <summary>
    /// Načte a zaregistruje do DI custom configuraci aplikace z appsettings.json
    /// </summary>
    /// <remarks>
    /// Konfigurace musí být v appsettings.json v objektu "AppConfiguration"
    /// </remarks>
    IGrpcServiceFluentBuilder<TConfiguration> AddApplicationConfiguration<TConfiguration>()
        where TConfiguration : class;

    /// <summary>
    /// Možnost registrace vlastních services nebo dalších dependences, které je potřeba registrovat do DI kontajneru.
    /// </summary>
    IGrpcServiceFluentBuilderStage2 Build(Action<WebApplicationBuilder> services);

    /// <summary>
    /// Registruje distribuovanou cache z CIS frameworku
    /// </summary>
    IGrpcServiceFluentBuilder AddDistributedCache();

    /// <summary>
    /// Zaregistruje custom error messages do FluentValidation.
    /// </summary>
    IGrpcServiceFluentBuilder AddErrorCodeMapper(IErrorCodesDictionary validationMessages);

    /// <summary>
    /// Registrace doménových nebo interních služeb.
    /// </summary>
    IGrpcServiceFluentBuilder AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder, ICisEnvironmentConfiguration> serviceBuilder);

    /// <summary>
    /// Registrace doménových nebo interních služeb.
    /// </summary>
    IGrpcServiceFluentBuilder AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder> serviceBuilder);

    /// <summary>
    /// Přidá podporu rollbacku z CIS frameworku
    /// </summary>
    IGrpcServiceFluentBuilder AddRollbackCapability();

    /// <summary>
    /// Přidá do aplikace gRPC / JSON Transcoding včetně SwaggerUI
    /// </summary>
    /// <param name="options">Možnost nastavení SwaggerUI, např. název API, XML komentáře...</param>
    IGrpcServiceFluentBuilder EnableJsonTranscoding(Action<FluentBuilderJsonTranscodingOptions> options);

    /// <summary>
    /// Ukončuje část konfigurace DI kontajneru a vrací builder pro vytváření middleware konfigurace.
    /// </summary>
    IGrpcServiceFluentBuilderStage2 Build();
}
