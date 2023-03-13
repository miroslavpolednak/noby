using CIS.Infrastructure.gRPC;
using CIS.InternalServices.NotificationService.Clients.Interfaces;
using CIS.InternalServices.NotificationService.Clients.Services;
using CIS.InternalServices.NotificationService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;

namespace CIS.InternalServices.NotificationService.Clients;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationClient(this IServiceCollection services)
    {
        services
            .AddSingleton<GenericClientExceptionInterceptor>()
            .AddScoped<ContextUserForwardingClientInterceptor>()
            .AddScoped<INotificationClient, NotificationClient>()
            .AddCodeFirstGrpcClient<INotificationService>((_, options) =>
            {
                // todo: uri from configuration
                options.Address = new Uri("https://localhost:5003");
            })
            .CisConfigureChannelWithoutCertificateValidation()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();

        return services;
    }
}