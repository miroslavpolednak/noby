using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.UserService.Clients;

public static class UserServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:UserService";

    public static IServiceCollection AddDomainService<TClient>(this IServiceCollection services)
        where TClient : IUserServiceClient
    {
        services.AddTransient<IUserServiceClient, Services.UserService>();
        services.AddCisGrpcClientUsingServiceDiscovery<Contracts.v1.UserService.UserServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddDomainService<TClient>(this IServiceCollection services, string serviceUrl)
        where TClient : IUserServiceClient
    {
        services.AddTransient<IUserServiceClient, Services.UserService>();
        services.AddCisGrpcClientUsingUrl<Contracts.v1.UserService.UserServiceClient>(serviceUrl);
        return services;
    }
}
