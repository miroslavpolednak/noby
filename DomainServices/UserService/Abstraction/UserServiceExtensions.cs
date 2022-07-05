using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.InternalServices.ServiceDiscovery.Abstraction;

namespace DomainServices.UserService.Abstraction;

public static class UserServiceExtensions
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
    => services
        .AddCisServiceDiscovery()
        .registerUriSettings()
        .registerServices()
        .registerGrpcServices();

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.UserService.UserServiceClient>(serviceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.UserService.UserServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:UserService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.v1.UserService.UserServiceClient>(url ?? throw new ArgumentNullException("url", "UserService URL can not be determined"));
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.TryAddTransient<IUserServiceAbstraction, Services.UserService>();

        // redis cache
        /*services.AddRedisGlobalCache<IUserServiceAbstraction>(provider =>
        {
            string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                .GetService(new("CIS:GlobalCache:Redis"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary)
                .GetAwaiter()
                .GetResult()?
                .ServiceUrl;
            return url ?? throw new ArgumentNullException("url", "Service Discovery can not find CIS:GlobalCache:Redis Proprietary service URL");
        }, "MPSS:");*/

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.UserService.UserServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.UserService.UserServiceClient, Contracts.v1.UserService.UserServiceClient>();
        }
        return services;
    }
}
