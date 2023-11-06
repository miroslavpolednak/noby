using CIS.Core.Configuration;
using CIS.Core.ErrorCodes;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;

namespace SharedComponents.GrpcServiceBuilderHelpers;

public interface IGrpcServiceFluentBuilder<TConfiguration>
    where TConfiguration : class
{
    /// <summary>
    /// Skipne z registrace funkcnost pro ziskani kontextoveho uzivatele sluzby. Je potreba kdyz dana sluzba nepouziva UserService.
    /// </summary>
    /// <remarks>Odstranuje z pipeline zaroven Audit logging.</remarks>
    IGrpcServiceFluentBuilder<TConfiguration> SkipServiceUserContext();

    /// <summary>
    /// Možnost změny nastavení gRCP služeb. Supluje .AddGrpc(o)
    /// </summary>
    IGrpcServiceFluentBuilder<TConfiguration> AddGrpcServiceOptions(Action<GrpcServiceOptions> changeOptions);

    /// <summary>
    /// Možnost registrace vlastních services nebo dalších dependences, které je potřeba registrovat do DI kontajneru.
    /// </summary>
    IGrpcServiceFluentBuilderStage2<TConfiguration> Build(Action<WebApplicationBuilder, TConfiguration> services);

    /// <summary>
    /// Možnost registrace vlastních services nebo dalších dependences, které je potřeba registrovat do DI kontajneru.
    /// </summary>
    IGrpcServiceFluentBuilderStage2<TConfiguration> Build(Action<WebApplicationBuilder> services);

    /// <summary>
    /// Registruje distribuovanou cache z CIS frameworku
    /// </summary>
    IGrpcServiceFluentBuilder<TConfiguration> AddDistributedCache();

    /// <summary>
    /// Zaregistruje custom error messages do FluentValidation.
    /// </summary>
    IGrpcServiceFluentBuilder<TConfiguration> AddErrorCodeMapper(IErrorCodesDictionary validationMessages);

    /// <summary>
    /// Registrace doménových nebo interních služeb.
    /// </summary>
    IGrpcServiceFluentBuilder<TConfiguration> AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder, ICisEnvironmentConfiguration> serviceBuilder);

    /// <summary>
    /// Registrace doménových nebo interních služeb.
    /// </summary>
    IGrpcServiceFluentBuilder<TConfiguration> AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder> serviceBuilder);

    /// <summary>
    /// Přidá podporu rollbacku z CIS frameworku
    /// </summary>
    IGrpcServiceFluentBuilder<TConfiguration> AddRollbackCapability();

    /// <summary>
    /// Přidá do aplikace gRPC / JSON Transcoding včetně SwaggerUI
    /// </summary>
    /// <param name="options">Možnost nastavení SwaggerUI, např. název API, XML komentáře...</param>
    IGrpcServiceFluentBuilder<TConfiguration> EnableJsonTranscoding(Action<FluentBuilderJsonTranscodingOptions> options);

    /// <summary>
    /// Ukončuje část konfigurace DI kontajneru a vrací builder pro vytváření middleware konfigurace.
    /// </summary>
    IGrpcServiceFluentBuilderStage2<TConfiguration> Build();
}