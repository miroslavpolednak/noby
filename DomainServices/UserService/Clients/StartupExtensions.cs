﻿using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.UserService.Clients;
using __Services = DomainServices.UserService.Clients.Services;
using __Contracts = DomainServices.UserService.Contracts;
using CIS.InternalServices;

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
        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.UserService.UserServiceClient>(ServiceName);
        return services.AddTransient<IUserServiceClient, __Services.UserService>();
    }

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl)
    {
        services.AddCisGrpcClientUsingUrl<__Contracts.v1.UserService.UserServiceClient>(serviceUrl);
        return services.AddTransient<IUserServiceClient, __Services.UserService>();
    }
}