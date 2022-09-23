using CIS.Infrastructure.gRPC;
using CIS.InternalServices.NotificationService.Client.Interfaces;
using CIS.InternalServices.NotificationService.Client.Services;
using CIS.InternalServices.NotificationService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;

namespace CIS.InternalServices.NotificationService.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationClient(this IServiceCollection services)
    {
        services
            .AddSingleton<GenericClientExceptionInterceptor>()
            .AddScoped<ContextUserForwardingClientInterceptor>()
            .AddScoped<INotificationClient, NotificationClient>()
            .AddCodeFirstGrpcClient<INotificationService>((provider, options) =>
            {
                // todo: uri from configuration
                options.Address = new Uri("https://localhost:5003");
            })
            .CisConfigureChannel()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();

        return services;
    }
}