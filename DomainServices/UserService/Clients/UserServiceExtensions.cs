using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using CIS.InternalServices.ServiceDiscovery.Abstraction;

namespace DomainServices.UserService.Clients;

public static class UserServiceExtensions
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.v1.UserService.UserServiceClient>(a =>
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.v1.UserService.UserServiceClient>("DS:UserService")
            .registerServices()
        );

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl)
        => services.TryAddGrpcClient<Contracts.v1.UserService.UserServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.v1.UserService.UserServiceClient>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddTransient<IUserServiceAbstraction, Services.UserService>();

        services.AddGrpcClientFromCisEnvironment<Contracts.v1.UserService.UserServiceClient>();
        
        return services;
    }
}
