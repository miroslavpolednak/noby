using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using __Contracts = DomainServices.UserService.Contracts;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.Infrastructure.Caching.Grpc;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:UserService";

    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();

        services.AddGrpcClientResponseCaching<UserService.Clients.v1.UserServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.UserService.UserServiceClient>(ServiceName);
        services.TryAddTransient<UserService.Clients.v1.IUserServiceClient, UserService.Clients.v1.UserServiceClient>();
        
        return services;
    }

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.UserService.UserServiceClient>(serviceUrl);
        services.TryAddTransient<UserService.Clients.v1.IUserServiceClient, UserService.Clients.v1.UserServiceClient>();

        return services;
    }
}
