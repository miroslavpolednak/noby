using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.gRPC;

public static class GrpcStartupExtensions
{
    public static IServiceCollection TryAddGrpcClient<TService>(this IServiceCollection services, Action<IServiceCollection> registration)
    {
        if (!services.Any(t => t.ServiceType == typeof(TService)))
        {
            registration(services);
        }
        return services;
    }

    public static IHttpClientBuilder AddGrpcClientFromCisEnvironment<TService>(this IServiceCollection services) 
        where TService : class
    {
        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();
        
        return services
            .AddGrpcClient<TService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TService>>();
                options.Address = serviceUri.Url;
            })
            .CisConfigureChannel()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();
    }

    public static IHttpClientBuilder AddGrpcClientFromCisEnvironment<TService, TServiceUriSettings>(this IServiceCollection services)
        where TService : class 
        where TServiceUriSettings : class
    {
        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();

        return services
            .AddGrpcClient<TService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TServiceUriSettings>>();
                options.Address = serviceUri.Url;
            })
            .CisConfigureChannel()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();
    }

    public static IServiceCollection AddGrpcServiceUriSettings<TService>(this IServiceCollection services, string serviceUrl)
        where TService : class
    {
        services.TryAddSingleton(new GrpcServiceUriSettings<TService>(serviceUrl));
        return services;
    }

    public static IHttpClientBuilder CisConfigureChannel(this IHttpClientBuilder builder)
        => builder.ConfigureChannel(configureChannel);

    public static IHttpClientBuilder AddCisCallCredentials(this IHttpClientBuilder builder)
        => builder.AddCallCredentials(addCredentials);

    private static Func<AuthInterceptorContext, Metadata, IServiceProvider, Task> addCredentials =
        (context, metadata, serviceProvider) =>
        {
            var configuration = serviceProvider.GetRequiredService<Core.Configuration.ICisEnvironmentConfiguration>();

            if (string.IsNullOrEmpty(configuration.InternalServicesLogin) || string.IsNullOrEmpty(configuration.InternalServicePassword))
                throw new System.Security.Authentication.InvalidCredentialException("InternalServicesLogin or InternalServicePassword is empty");
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{configuration.InternalServicesLogin}:{configuration.InternalServicePassword}");

            // add authentication header
            metadata.Add("authorization", $"Basic {Convert.ToBase64String(plainTextBytes)}");

            return Task.CompletedTask;
        };

    private static Action<GrpcChannelOptions> configureChannel =
        (GrpcChannelOptions options) =>
        {
            HttpClientHandler httpHandler = new()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            options.HttpHandler = httpHandler;
        };
}