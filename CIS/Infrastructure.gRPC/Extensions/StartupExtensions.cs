using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.gRPC;

public static class StartupExtensions
{
    public static IHttpClientBuilder AddCisGrpcClientUsingServiceDiscovery<TService>(this IServiceCollection services, in string serviceName, bool validateServiceCertificate = false)
        where TService : class
    {
        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TService>>(new Configuration.GrpcServiceUriSettingsServiceDiscovery<TService>(serviceName));
        return services.AddCisGrpcClientInner<TService, TService>(validateServiceCertificate, true);
    }
    
    public static IHttpClientBuilder AddCisGrpcClientUsingUrl<TService>(this IServiceCollection services, in string serviceUrl, bool validateServiceCertificate = false)
        where TService : class
    {
        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TService>>(new Configuration.GrpcServiceUriSettingsDirect<TService>(serviceUrl));
        return services.AddCisGrpcClientInner<TService, TService>(validateServiceCertificate, true);
    }

    public static IHttpClientBuilder AddCisGrpcClientUsingServiceDiscovery<TService, TServiceUriSettings>(this IServiceCollection services, in string serviceName, bool validateServiceCertificate = false)
        where TService : class
        where TServiceUriSettings : class
    {
        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TServiceUriSettings>>(new Configuration.GrpcServiceUriSettingsServiceDiscovery<TServiceUriSettings>(serviceName));
        return services.AddCisGrpcClientInner<TService, TServiceUriSettings>(validateServiceCertificate, true);
    }

    public static IHttpClientBuilder AddCisGrpcClientUsingUrl<TService, TServiceUriSettings>(this IServiceCollection services, in string serviceUrl, bool validateServiceCertificate = false)
        where TService : class
        where TServiceUriSettings : class
    {
        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TServiceUriSettings>>(new Configuration.GrpcServiceUriSettingsDirect<TServiceUriSettings>(serviceUrl));
        return services.AddCisGrpcClientInner<TService, TServiceUriSettings>(validateServiceCertificate, true);
    }

    /// <summary>
    /// Nepouzivat primo, je public pouze pro ServiceDiscovery nebo jine specialni pripady.
    /// </summary>
    public static IHttpClientBuilder AddCisGrpcClientInner<TService, TServiceUriSettings>(this IServiceCollection services, bool validateServiceCertificate, bool forwardClientHeaders)
        where TService : class
        where TServiceUriSettings : class
    {
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddScoped<ContextUserForwardingClientInterceptor>();

        // register service
        var builder = services
            .AddGrpcClient<TService>((provider, options) =>
            {
                options.Address = provider.GetRequiredService<Configuration.IGrpcServiceUriSettings<TServiceUriSettings>>().ServiceUrl;
            })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddCisCallCredentials();

        if (forwardClientHeaders)
            builder.AddInterceptor<ContextUserForwardingClientInterceptor>();

        if (!validateServiceCertificate)
            builder.CisConfigureChannelWithoutCertificateValidation();

        return builder;
    }
}
