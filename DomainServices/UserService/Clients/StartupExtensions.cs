using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.UserService.Clients;
using __Services = DomainServices.UserService.Clients.Services;
using __Contracts = DomainServices.UserService.Contracts;
using CIS.InternalServices.ServiceDiscovery.Clients;

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
        services.AddTransient<IUserServiceClient, __Services.UserService>();
        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.UserService.UserServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IUserServiceClient, __Services.UserService>();
        services.AddCisGrpcClientUsingUrl<__Contracts.v1.UserService.UserServiceClient>(serviceUrl);
        return services;
    }
}
