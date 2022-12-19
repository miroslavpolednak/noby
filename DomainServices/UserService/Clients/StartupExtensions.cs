using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.UserService.Clients;
using __Services = DomainServices.UserService.Clients.Services;
using __Contracts = DomainServices.UserService.Contracts;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.UserService.UserServiceClient>(ServiceName);
        services.TryAddTransient<IUserServiceClient, __Services.UserService>();
        return services;
    }

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.UserService.UserServiceClient>(serviceUrl);
        services.TryAddTransient<IUserServiceClient, __Services.UserService>();
        return services;
    }
}
