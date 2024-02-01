using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace SharedComponents.GrpcServiceBuilderHelpers;

internal sealed class GrpcServiceFluentBuilderStage2<TConfiguration>
    : IGrpcServiceFluentBuilderStage2<TConfiguration>
    where TConfiguration : class
{
    public IGrpcServiceFluentBuilderStage2<TConfiguration> MapGrpcServices(Action<IEndpointRouteBuilder, TConfiguration> endpointRouter)
    {
        _settings.MapGrpcServicesT = endpointRouter;
        return this;
    }

    public IGrpcServiceFluentBuilderStage2<TConfiguration> UseMiddlewares(Action<WebApplication, TConfiguration> middlewareBuilder)
    {
        _settings.UseMiddlewaresT = middlewareBuilder;
        return this;
    }

    public void Run()
    {
        var runner = new GrpcServiceBuilderRunner<TConfiguration>(_settings, false);
        runner.Run();
    }

    private readonly GrpcServiceBuilderSettings<TConfiguration> _settings;

    internal GrpcServiceFluentBuilderStage2(GrpcServiceBuilderSettings<TConfiguration> settings)
    {
        _settings = settings;
    }
}

internal sealed class GrpcServiceFluentBuilderStage2
    : IGrpcServiceFluentBuilderStage2
{
    public IGrpcServiceFluentBuilderStage2 MapGrpcServices(Action<IEndpointRouteBuilder> endpointRouter)
    {
        _settings.MapGrpcServices = endpointRouter;
        return this;
    }

    public IGrpcServiceFluentBuilderStage2 UseMiddlewares(Action<WebApplication> middlewareBuilder)
    {
        _settings.UseMiddlewares = middlewareBuilder;
        return this;
    }

    public void Run()
    {
        var runner = new GrpcServiceBuilderRunner<GrpcServiceBuilderSettings>(_settings.CreateGenericCopy<GrpcServiceBuilderSettings>(), true);
        runner.Run();
    }

    private readonly GrpcServiceBuilderSettings _settings;

    internal GrpcServiceFluentBuilderStage2(GrpcServiceBuilderSettings settings)
    {
        _settings = settings;
    }
}
