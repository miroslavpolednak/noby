using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;

namespace SharedComponents.GrpcServiceBuilderHelpers;

internal abstract class GrpcServiceFluentBuilderBase
{
    protected void IntEnableJsonTranscoding(Action<JsonTranscodingOptions> options)
    {
        if (_settings.EnableJsonTranscoding)
        {
            throw new CisConfigurationException(0, "GrpcServiceBuilder: JsonTranscoding already set");
        }

        _settings.EnableJsonTranscoding = true;
        // vychozi nastaveni transcodingu
        _settings.TranscodingOptions = new JsonTranscodingOptions();
        // custom uprava nastaveni
        options(_settings.TranscodingOptions);
    }

    protected void IntAddDistributedCache()
    {
        _settings.AddDistributedCache = true;
    }

    protected void IntAddCustomServices(Action<WebApplicationBuilder> services)
    {
        if (_settings.AddCustomServices is not null)
        {
            throw new CisConfigurationException(0, "GrpcServiceBuilder: CustomServices already set");
        }

        _settings.AddCustomServices = services;
    }

    protected void IntAddErrorCodeMapper(CIS.Core.ErrorCodes.IErrorCodesDictionary validationMessages)
    {
        if (_settings.ErrorCodeMapperMessages is not null)
        {
            throw new CisConfigurationException(0, "GrpcServiceBuilder: ErrorCodeMapper already set");
        }

        _settings.ErrorCodeMapperMessages = validationMessages;
    }

    protected void IntAddRollbackCapability()
    {
        _settings.AddRollbackCapability = true;
    }

    protected void IntAddRequiredServices(Action<GrpcServiceRequiredServicesBuilder> serviceBuilder)
    {
        if (_settings.RequiredServicesBuilder is not null)
        {
            throw new CisConfigurationException(0, "GrpcServiceBuilder: RequiredServices already set");
        }

        _settings.RequiredServicesBuilder = new GrpcServiceRequiredServicesBuilder();
        serviceBuilder(_settings.RequiredServicesBuilder);
    }

    protected void IntAddRequiredServices(Action<GrpcServiceRequiredServicesBuilder, ICisEnvironmentConfiguration> serviceBuilder)
    {
        if (_settings.RequiredServicesBuilder is not null)
        {
            throw new CisConfigurationException(0, "GrpcServiceBuilder: RequiredServices already set");
        }

        _settings.RequiredServicesBuilder = new GrpcServiceRequiredServicesBuilder();
        serviceBuilder(_settings.RequiredServicesBuilder, _settings.EnvironmentConfiguration);
    }

    private readonly GrpcServiceBuilderSettings _settings;

    internal GrpcServiceFluentBuilderBase(GrpcServiceBuilderSettings settings)
    {
        _settings = settings;
    }
}

