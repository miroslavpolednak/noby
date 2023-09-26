using Microsoft.AspNetCore.Routing;

namespace SharedComponents.GrpcServiceBuilderHelpers;

public interface IGrpcServiceFluentBuilderStage2<TConfiguration>
    where TConfiguration : class
{
    /// <summary>
    /// Možnost pro registraci gRPC služeb / endpointů
    /// </summary>
    IGrpcServiceBuilderRunner<TConfiguration> MapGrpcServices(Action<IEndpointRouteBuilder> endpointRouter);
}

public interface IGrpcServiceFluentBuilderStage2
{
    /// <summary>
    /// Možnost pro registraci gRPC služeb / endpointů
    /// </summary>
    IGrpcServiceBuilderRunner MapGrpcServices(Action<IEndpointRouteBuilder> endpointRouter);
}