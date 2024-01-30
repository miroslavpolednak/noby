using CIS.Core.Configuration;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace SharedComponents.GrpcServiceBuilderHelpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal sealed class GrpcServiceBuilderSettings<TConfiguration>
    : GrpcServiceBuilderSettings
{
    public TConfiguration? Configuration;
    public Action<WebApplicationBuilder, TConfiguration>? AddCustomServicesWithConfiguration;
    public Action<IEndpointRouteBuilder, TConfiguration>? MapGrpcServicesT;
    public Action<WebApplication, TConfiguration>? UseMiddlewaresT;
}

internal class GrpcServiceBuilderSettings
{
    public Type MainAssembly;
    public WebApplicationBuilder Builder;
    public ICisEnvironmentConfiguration EnvironmentConfiguration;
    public bool RunAsWindowsService;

    public bool SkipServiceUserContext;
    public bool AddDistributedCache;
    public bool EnableJsonTranscoding;
    public FluentBuilderJsonTranscodingOptions? TranscodingOptions;
    public CIS.Core.ErrorCodes.IErrorCodesDictionary? ErrorCodeMapperMessages;
    public bool AddRollbackCapability;
    public Action<WebApplicationBuilder>? AddCustomServices;
    public GrpcServiceRequiredServicesBuilder? RequiredServicesBuilder;
    public Action<GrpcServiceOptions>? AddGrpcServiceOptions;

    public Action<IEndpointRouteBuilder>? MapGrpcServices;
    public Action<WebApplication>? UseMiddlewares;

    public GrpcServiceBuilderSettings<TConfiguration> CreateGenericCopy<TConfiguration>()
        where TConfiguration : class
    {
        // create new settings
        var newSettings = new GrpcServiceBuilderSettings<TConfiguration>
        {
            SkipServiceUserContext = this.SkipServiceUserContext,
            AddGrpcServiceOptions = this.AddGrpcServiceOptions,
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
            MapGrpcServices = this.MapGrpcServices,
            UseMiddlewares = this.UseMiddlewares
        };

        return newSettings;
    }
}
