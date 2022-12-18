using CIS.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Extension metody do startupu aplikace pro registraci gRPC služeb.
/// </summary>
public static class StartupExtensions
{
    public static void TryAddCisGrpcClientUsingServiceDiscovery<TService>(this IServiceCollection services, in string serviceName, bool validateServiceCertificate = false)
        where TService : class
    {
        if (services.AlreadyRegistered<Configuration.IGrpcServiceUriSettings<TService>>())
            return;

        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TService>>(new Configuration.GrpcServiceUriSettingsServiceDiscovery<TService>(serviceName));
        services.AddCisGrpcClientInner<TService, TService>(validateServiceCertificate, true);
    }
    
    public static void TryAddCisGrpcClientUsingUrl<TService>(this IServiceCollection services, in string serviceUrl, bool validateServiceCertificate = false)
        where TService : class
    {
        if (services.AlreadyRegistered<Configuration.IGrpcServiceUriSettings<TService>>())
            return;

        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TService>>(new Configuration.GrpcServiceUriSettingsDirect<TService>(serviceUrl));
        services.AddCisGrpcClientInner<TService, TService>(validateServiceCertificate, true);
    }

    public static void TryAddCisGrpcClientUsingServiceDiscovery<TService, TServiceUriSettings>(this IServiceCollection services, in string serviceName, bool validateServiceCertificate = false)
        where TService : class
        where TServiceUriSettings : class
    {
        if (services.AlreadyRegistered<Configuration.IGrpcServiceUriSettings<TServiceUriSettings>>())
            return;

        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TServiceUriSettings>>(new Configuration.GrpcServiceUriSettingsServiceDiscovery<TServiceUriSettings>(serviceName));
        services.AddCisGrpcClientInner<TService, TServiceUriSettings>(validateServiceCertificate, true);
    }

    public static void TryAddCisGrpcClientUsingUrl<TService, TServiceUriSettings>(this IServiceCollection services, in string serviceUrl, bool validateServiceCertificate = false)
        where TService : class
        where TServiceUriSettings : class
    {
        if (services.AlreadyRegistered<Configuration.IGrpcServiceUriSettings<TServiceUriSettings>>())
            return;

        services.TryAddSingleton<Configuration.IGrpcServiceUriSettings<TServiceUriSettings>>(new Configuration.GrpcServiceUriSettingsDirect<TServiceUriSettings>(serviceUrl));
        services.AddCisGrpcClientInner<TService, TServiceUriSettings>(validateServiceCertificate, true);
    }

    /// <summary>
    /// Nepouzivat primo, je public pouze pro ServiceDiscovery nebo jine specialni pripady.
    /// </summary>
    public static IHttpClientBuilder AddCisGrpcClientInner<TService, TServiceUriSettings>(this IServiceCollection services, bool validateServiceCertificate, bool forwardClientHeaders)
        where TService : class
        where TServiceUriSettings : class
    {
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddTransient<ContextUserForwardingClientInterceptor>();

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

    /// <summary>
    /// Zaregistruje do DI:
    /// - MediatR
    /// - FluentValidation through MediatR pipelines
    /// </summary>
    /// <param name="assemblyType">Typ, který je v hlavním projektu - typicky Program.cs</param>
    public static IServiceCollection AddCisGrpcInfrastructure(this IServiceCollection services, Type assemblyType)
    {
        services
            .AddMediatR(assemblyType.Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(Validation.GrpcValidationBehavior<,>));

        // add validators
        services.Scan(selector => selector
            .FromAssembliesOf(assemblyType)
            .AddClasses(x => x.AssignableTo(typeof(FluentValidation.IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
