using CIS.Core.Configuration;
using CIS.Core.ErrorCodes;
using Microsoft.AspNetCore.Builder;

namespace SharedComponents.GrpcServiceBuilderHelpers;

public interface IGrpcServiceFluentBuilder
{
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
    IGrpcServiceFluentBuilder AddCustomServices(Action<WebApplicationBuilder> services);

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
    IGrpcServiceFluentBuilderStage2 AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder, ICisEnvironmentConfiguration> serviceBuilder);

    /// <summary>
    /// Registrace doménových nebo interních služeb.
    /// </summary>
    IGrpcServiceFluentBuilderStage2 AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder> serviceBuilder);

    /// <summary>
    /// Přidá podporu rollbacku z CIS frameworku
    /// </summary>
    IGrpcServiceFluentBuilder AddRollbackCapability();

    /// <summary>
    /// Přidá do aplikace gRPC / JSON Transcoding včetně SwaggerUI
    /// </summary>
    /// <param name="options">Možnost nastavení SwaggerUI, např. název API, XML komentáře...</param>
    IGrpcServiceFluentBuilder EnableJsonTranscoding(Action<JsonTranscodingOptions> options);

    /// <summary>
    /// Pokud není potřeba registrace žádné doménové nebo interní služby, je to nutné explicitně vyjádřit touto metodou.
    /// </summary>
    IGrpcServiceFluentBuilderStage2 SkipRequiredServices();
}
