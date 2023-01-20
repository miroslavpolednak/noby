﻿using CIS.InternalServices;
using DomainServices.DocumentOnSAService.Clients.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using __Services = DomainServices.DocumentOnSAService.Clients.Services;
using __Contracts = DomainServices.DocumentOnSAService.Contracts;
using CIS.Infrastructure.gRPC;

namespace DomainServices.DocumentOnSAService.Clients;

public static class DocumentOnSAServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:DocumentOnSAService";

    public static IServiceCollection AddDocumentOnSAService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();

        services.AddTransient<IDocumentOnSAServiceClient, __Services.DocumentOnSAService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.DocumentOnSAService.DocumentOnSAServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddDocumentOnSAService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IDocumentOnSAServiceClient, __Services.DocumentOnSAService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.DocumentOnSAService.DocumentOnSAServiceClient>(serviceUrl);
        return services;
    }
}
