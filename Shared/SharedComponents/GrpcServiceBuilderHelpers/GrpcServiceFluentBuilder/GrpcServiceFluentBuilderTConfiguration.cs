﻿using CIS.Core.Configuration;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;

namespace SharedComponents.GrpcServiceBuilderHelpers;

internal sealed class GrpcServiceFluentBuilder<TConfiguration>
    : GrpcServiceFluentBuilderBase, IGrpcServiceFluentBuilder<TConfiguration>
    where TConfiguration : class
{
    public IGrpcServiceFluentBuilder<TConfiguration> SkipServiceUserContext()
    {
        IntSkipServiceUserContext();
        return this;
    }

    public IGrpcServiceFluentBuilder<TConfiguration> AddGrpcServiceOptions(Action<GrpcServiceOptions> changeOptions)
    {
        IntAddGrpcServiceOptions(changeOptions);
        return this;
    }

    public IGrpcServiceFluentBuilderStage2<TConfiguration> Build(Action<WebApplicationBuilder, TConfiguration> services)
    {
        if (_settings.AddCustomServices is not null)
        {
            throw new CisConfigurationException(0, "GrpcServiceBuilder: AddCustomServices already set while trying to set its generic version");
        }

        _settings.AddCustomServicesWithConfiguration = services;
        return Build();
    }

    public IGrpcServiceFluentBuilder<TConfiguration> EnableJsonTranscoding(Action<FluentBuilderJsonTranscodingOptions> options)
    {
        IntEnableJsonTranscoding(options);
        return this;
    }

    public IGrpcServiceFluentBuilder<TConfiguration> AddDistributedCache()
    {
        IntAddDistributedCache();
        return this;
    }

    public IGrpcServiceFluentBuilderStage2<TConfiguration> Build(Action<WebApplicationBuilder> services)
    {
        IntAddCustomServices(services);
        return Build();
    }

    public IGrpcServiceFluentBuilder<TConfiguration> AddErrorCodeMapper(CIS.Core.ErrorCodes.IErrorCodesDictionary validationMessages)
    {
        IntAddErrorCodeMapper(validationMessages);
        return this;
    }

    public IGrpcServiceFluentBuilder<TConfiguration> AddRollbackCapability()
    {
        IntAddRollbackCapability();
        return this;
    }

    public IGrpcServiceFluentBuilder<TConfiguration> AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder> serviceBuilder)
    {
        IntAddRequiredServices(serviceBuilder);
        return this;
    }

    public IGrpcServiceFluentBuilder<TConfiguration> AddRequiredServices(Action<GrpcServiceRequiredServicesBuilder, ICisEnvironmentConfiguration> serviceBuilder)
    {
        IntAddRequiredServices(serviceBuilder);
        return this;
    }

    public IGrpcServiceFluentBuilderStage2<TConfiguration> Build()
    {
        return new GrpcServiceFluentBuilderStage2<TConfiguration>(_settings);
    }

    private readonly GrpcServiceBuilderSettings<TConfiguration> _settings;

    internal GrpcServiceFluentBuilder(GrpcServiceBuilderSettings<TConfiguration> settings)
        : base(settings)
    {
        _settings = settings;
    }
}
