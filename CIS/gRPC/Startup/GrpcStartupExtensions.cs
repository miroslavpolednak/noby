using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.gRPC;

public static class GrpcStartupExtensions
{
    /// <summary>
    /// Umozni nasatavit kestrel custom konfiguracnim souborem.
    /// Vychozi nazev pro konfiguracni soubor je "kestrel.json". Soubor musi obsahovat root node "CustomeKestrel", pod kterym je struktura CIS.Core.Configuration.KestrelConfiguration.
    /// </summary>
    public static WebApplicationBuilder UseKestrelWithCustomConfiguration(this WebApplicationBuilder builder, string configurationFilename = "kestrel.json")
    {
        string devFilename = Path.GetFileNameWithoutExtension(configurationFilename) + ".development.json";

        Core.Configuration.KestrelConfiguration kestrelConfiguration = new();
        builder.Configuration.AddJsonFile(configurationFilename, false);
        builder.Configuration.AddJsonFile(devFilename, true);
        builder.Configuration.GetSection("CustomKestrel").Bind(kestrelConfiguration);

        if (kestrelConfiguration?.Endpoints == null)
            throw new ArgumentNullException($"Kestrel configuration file ({configurationFilename}) not found or configuration is not valid (does not contain root element CustomKestrel?)");
        
        builder.WebHost.UseKestrel(serverOptions =>
        {
            serverOptions.ConfigureEndpointDefaults(opts =>
            {
                if (kestrelConfiguration.Certificate != null)
                    opts.UseHttps(kestrelConfiguration.Certificate?.Path ?? throw new Core.Exceptions.CisConfigurationNotFound("CustomKestrel.Certificate.Path"), kestrelConfiguration.Certificate.Password);
            });

            kestrelConfiguration.Endpoints?.ForEach(endpoint =>
            {
                serverOptions.ListenAnyIP(endpoint.Port, listenOptions =>
                {
                    listenOptions.Protocols = (Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols)endpoint.Protocol;
                });
            });
        });

        return builder;
    }

    public static IHttpClientBuilder AddGrpcClientFromCisEnvironment<TService>(this IServiceCollection services) 
        where TService : class
    {
        return services.AddGrpcClient<TService>((provider, options) =>
        {
            var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TService>>();
            options.Address = serviceUri.Url;
        })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true);
    }

    public static IHttpClientBuilder AddGrpcClientFromCisEnvironment<TService, TServiceUriSettings>(this IServiceCollection services)
        where TService : class 
        where TServiceUriSettings : class
    {
        return services.AddGrpcClient<TService>((provider, options) =>
        {
            var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TServiceUriSettings>>();
            options.Address = serviceUri.Url;
        })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true);
    }

    public static IHttpClientBuilder ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<TService>(this IHttpClientBuilder builder)
        where TService : class
    {
        return builder.ConfigurePrimaryHttpMessageHandler((provider) =>
        {
            var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TService>>();

            HttpClientHandler httpHandler = new();
            // neduveryhodny certifikat
            if (serviceUri.IsInvalidCertificateAllowed)
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            return httpHandler;
        });
    }

    public static IServiceCollection AddGrpcServiceUriSettings<TService>(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        where TService : class
    {
        services.TryAddSingleton(new GrpcServiceUriSettings<TService>(serviceUrl, isInvalidCertificateAllowed));
        return services;
    }
}
