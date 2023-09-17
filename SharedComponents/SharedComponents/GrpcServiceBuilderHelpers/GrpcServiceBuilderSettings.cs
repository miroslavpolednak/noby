using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace SharedComponents.GrpcServiceBuilderHelpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal sealed class GrpcServiceBuilderSettings<TConfiguration>
    : GrpcServiceBuilderSettings
{
    public TConfiguration? Configuration;
    public Action<WebApplicationBuilder, TConfiguration>? AddCustomServicesWithConfiguration;
}

internal class GrpcServiceBuilderSettings
{
    public Type MainAssembly;
    public WebApplicationBuilder Builder;
    public ICisEnvironmentConfiguration EnvironmentConfiguration;
    public bool RunAsWindowsService;

    public bool AddDistributedCache;
    public bool EnableJsonTranscoding;
    public JsonTranscodingOptions? TranscodingOptions;
    public CIS.Core.ErrorCodes.IErrorCodesDictionary? ErrorCodeMapperMessages;
    public bool AddRollbackCapability;
    public Action<WebApplicationBuilder>? AddCustomServices;
    public GrpcServiceRequiredServicesBuilder? RequiredServicesBuilder;

    public Action<IEndpointRouteBuilder>? MapGrpcServices;

    public GrpcServiceBuilderSettings<TConfiguration> CreateGenericCopy<TConfiguration>()
        where TConfiguration : class
    {
        // create new settings
        var newSettings = new GrpcServiceBuilderSettings<TConfiguration>
        {
            AddDistributedCache = this.AddDistributedCache,
            AddCustomServices = this.AddCustomServices,
            RequiredServicesBuilder = this.RequiredServicesBuilder,
            RunAsWindowsService = this.RunAsWindowsService,
            AddRollbackCapability = this.AddRollbackCapability,
            Builder = this.Builder,
            EnvironmentConfiguration = this.EnvironmentConfiguration,
            EnableJsonTranscoding = this.EnableJsonTranscoding,
            MainAssembly = this.MainAssembly,
            TranscodingOptions = this.TranscodingOptions,
            ErrorCodeMapperMessages = this.ErrorCodeMapperMessages,
            MapGrpcServices = this.MapGrpcServices
        };

        return newSettings;
    }
}
