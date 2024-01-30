using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace SharedComponents.GrpcServiceBuilderHelpers;

public interface IGrpcServiceFluentBuilderStage2<TConfiguration>
    where TConfiguration : class
{
    /// <summary>
    /// Možnost pro registraci gRPC služeb / endpointů
    /// </summary>
    IGrpcServiceFluentBuilderStage2<TConfiguration> MapGrpcServices(Action<IEndpointRouteBuilder, TConfiguration> endpointRouter);

    /// <summary>
    /// Umožňuje zaregistrovat middleware, které se spustí před gRPC službami.
    /// </summary>
    IGrpcServiceFluentBuilderStage2<TConfiguration> UseMiddlewares(Action<WebApplication, TConfiguration> endpointRouter);

    /// <summary>
    /// Spustí aplikaci. Provádí registraci služeb, následně vytváří WebApplication a registruje middlewares.
    /// </summary>
    /// <remarks>
    /// Celý startup aplikace běží v try-catch bloku, který je v případě pádu aplikace logován.
    /// </remarks>
    void Run();
}

public interface IGrpcServiceFluentBuilderStage2
{
    /// <summary>
    /// Možnost pro registraci gRPC služeb / endpointů
    /// </summary>
    IGrpcServiceFluentBuilderStage2 MapGrpcServices(Action<IEndpointRouteBuilder> endpointRouter);

    /// <summary>
    /// Umožňuje zaregistrovat middleware, které se spustí před gRPC službami.
    /// </summary>
    IGrpcServiceFluentBuilderStage2 UseMiddlewares(Action<WebApplication> endpointRouter);

    /// <summary>
    /// Spustí aplikaci. Provádí registraci služeb, následně vytváří WebApplication a registruje middlewares.
    /// </summary>
    /// <remarks>
    /// Celý startup aplikace běží v try-catch bloku, který je v případě pádu aplikace logován.
    /// </remarks>
    void Run();
}