using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using _Contracts = DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices.HouseholdService.Clients;

public static class DocumentArchiveServiceExtensions
{
    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services)
        => services
            .TryAddGrpcClient<_Contracts.IDocumentArchiveService>(a =>
                a.AddGrpcServiceUriSettingsFromServiceDiscovery<_Contracts.IDocumentArchiveService>("DS:DocumentArchiveService")
            .registerServices()
        );

    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, string serviceUrl)
        => services
            .TryAddGrpcClient<_Contracts.IDocumentArchiveService>(a =>
                a.AddGrpcServiceUriSettings<_Contracts.IDocumentArchiveService>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();

        services.register<_Contracts.IDocumentArchiveService, DocumentArchiveService.Clients.IDocumentArchiveServiceClient, DocumentArchiveService.Clients.Services.DocumentArchiveServiceClient>();

        return services;
    }

    static void register<IService, IAbstraction, TAbstraction>(this IServiceCollection services)
        where IService : class
        where IAbstraction : class
        where TAbstraction : class
        => services
            .AddTransient(typeof(IAbstraction), typeof(TAbstraction))
            .AddCodeFirstGrpcClient<IService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<_Contracts.IDocumentArchiveService>>();
                options.Address = serviceUri.Url;
            })
            .CisConfigureChannel()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();
}
