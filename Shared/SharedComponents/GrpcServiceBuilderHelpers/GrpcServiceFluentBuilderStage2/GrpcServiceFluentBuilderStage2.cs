using Microsoft.AspNetCore.Routing;

namespace SharedComponents.GrpcServiceBuilderHelpers;

internal sealed class GrpcServiceFluentBuilderStage2<TConfiguration>
    : GrpcServiceFluentBuilderStage2Base, IGrpcServiceFluentBuilderStage2<TConfiguration>
    where TConfiguration : class
{
    public IGrpcServiceBuilderRunner<TConfiguration> MapGrpcServices(Action<IEndpointRouteBuilder> endpointRouter)
    {
        IntMapGrpcServices(endpointRouter);
        return new GrpcServiceBuilderRunner<TConfiguration>(_settings);
    }

    private readonly GrpcServiceBuilderSettings<TConfiguration> _settings;

    internal GrpcServiceFluentBuilderStage2(GrpcServiceBuilderSettings<TConfiguration> settings)
        : base(settings)
    {
        _settings = settings;
    }
}

internal sealed class GrpcServiceFluentBuilderStage2
    : GrpcServiceFluentBuilderStage2Base, IGrpcServiceFluentBuilderStage2
{
    public IGrpcServiceBuilderRunner MapGrpcServices(Action<IEndpointRouteBuilder> endpointRouter)
    {
        IntMapGrpcServices(endpointRouter);
        return new GrpcServiceBuilderRunner<GrpcServiceBuilderSettings>(_settings.CreateGenericCopy<GrpcServiceBuilderSettings>());
    }

    private readonly GrpcServiceBuilderSettings _settings;

    internal GrpcServiceFluentBuilderStage2(GrpcServiceBuilderSettings settings)
        : base(settings)
    {
        _settings = settings;
    }
}

internal abstract class GrpcServiceFluentBuilderStage2Base
{
    protected void IntMapGrpcServices(Action<IEndpointRouteBuilder> endpointRouter)
    {
        _settings.MapGrpcServices = endpointRouter;
    }

    private readonly GrpcServiceBuilderSettings _settings;

    internal GrpcServiceFluentBuilderStage2Base(GrpcServiceBuilderSettings settings)
    {
        _settings = settings;
    }
}