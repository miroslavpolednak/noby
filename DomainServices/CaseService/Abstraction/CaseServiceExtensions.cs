using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.CaseService.Abstraction;

public static class CaseServiceExtensions
{
    public static IServiceCollection AddCaseService(this IServiceCollection services)
    => services
        .AddCisServiceDiscovery()
        .registerUriSettings()
        .registerServices()
        .registerGrpcServices();

    public static IServiceCollection AddCaseService(this IServiceCollection services, string serviceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.CaseService.CaseServiceClient>(serviceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.CaseService.CaseServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:CaseService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.v1.CaseService.CaseServiceClient>(url ?? throw new ArgumentNullException("url", "CaseService URL can not be determined"));
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register service
        services.TryAddTransient<ICaseServiceAbstraction, Services.CaseService>();

        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.CaseService.CaseServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.CaseService.CaseServiceClient>()
                .AddInterceptor<ExceptionInterceptor>();
        }
        return services;
    }
}
