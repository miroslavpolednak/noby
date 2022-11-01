using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.CaseService.Clients;

public static class CaseServiceExtensions
{
    public static IServiceCollection AddCaseService(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.v1.CaseService.CaseServiceClient>(a =>
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.v1.CaseService.CaseServiceClient>("DS:CaseService")
            .registerServices()
        );

    public static IServiceCollection AddCaseService(this IServiceCollection services, string serviceUrl)
        => services.TryAddGrpcClient<Contracts.v1.CaseService.CaseServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.v1.CaseService.CaseServiceClient>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register service
        services.AddTransient<ICaseServiceClient, Services.CaseService>();

        // exception handling
        services.AddSingleton<ExceptionInterceptor>();

        services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.CaseService.CaseServiceClient>()
                .AddInterceptor<ExceptionInterceptor>();

        return services;
    }
}
