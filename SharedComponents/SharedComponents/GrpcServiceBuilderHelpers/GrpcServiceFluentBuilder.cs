using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedComponents.GrpcServiceBuilderHelpers;

internal class GrpcServiceFluentBuilder
    : GrpcServiceFluentBuilderBase, IGrpcServiceFluentBuilder
{
    public IGrpcServiceFluentBuilder<TConfiguration> AddApplicationConfiguration<TConfiguration>()
        where TConfiguration : class
    {
        var appConfiguration = _settings.Builder
            .Configuration
            .GetSection("AppConfiguration")
            .Get<TConfiguration>()
            ?? throw new CisConfigurationNotFound("AppConfiguration");

        _settings.Builder.Services.AddSingleton(appConfiguration);
        var newSettings = _settings.CreateGenericCopy<TConfiguration>();
        newSettings.Configuration = appConfiguration;

        return new GrpcServiceFluentBuilder<TConfiguration>(newSettings);
    }

    public IGrpcServiceFluentBuilder EnableJsonTranscoding(Action<JsonTranscodingOptions> options)
    {
        IntEnableJsonTranscoding(options);
        return this;
    }

    public IGrpcServiceFluentBuilder AddDistributedCache()
    {
        IntAddDistributedCache();
        return this;
    }

    public IGrpcServiceFluentBuilder AddCustomServices(Action<WebApplicationBuilder> services)
    {
        IntAddCustomServices(services);
        return this;
    }

    public IGrpcServiceFluentBuilder AddErrorCodeMapper(CIS.Core.ErrorCodes.IErrorCodesDictionary validationMessages)
    {
        IntAddErrorCodeMapper(validationMessages);
        return this;
    }

    public IGrpcServiceFluentBuilder AddRollbackCapability()
    {
        IntAddRollbackCapability();
        return this;
    }

    public IGrpcServiceFluentBuilderStage2 AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder> serviceBuilder)
    {
        IntAddRequiredServices(serviceBuilder);
        return new GrpcServiceFluentBuilderStage2(_settings);
    }

    public IGrpcServiceFluentBuilderStage2 AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder, ICisEnvironmentConfiguration> serviceBuilder)
    {
        IntAddRequiredServices(serviceBuilder);
        return new GrpcServiceFluentBuilderStage2(_settings);
    }

    public IGrpcServiceFluentBuilderStage2 SkipRequiredServices()
    {
        return new GrpcServiceFluentBuilderStage2(_settings);
    }

    private readonly GrpcServiceBuilderSettings _settings;

    internal GrpcServiceFluentBuilder(GrpcServiceBuilderSettings settings)
        : base(settings)
    {
        _settings = settings;
    }
}