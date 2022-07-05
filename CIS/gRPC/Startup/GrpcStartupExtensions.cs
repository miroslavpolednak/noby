using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Cryptography.X509Certificates;

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
                // pouzit k vytvoreni SSL certifikat
                if (kestrelConfiguration.Certificate != null)
                {
                    switch (kestrelConfiguration.Certificate.Location)
                    {
                        // ulozen na filesystemu
                        case Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes.FileSystem:
                            opts.UseHttps(kestrelConfiguration.Certificate?.Path ?? throw new Core.Exceptions.CisConfigurationNotFound("CustomKestrel.Certificate.Path"), kestrelConfiguration.Certificate.Password);
                            break;

                        // ulozen v certstore
                        case Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes.CertStore:
                            using (var store = new X509Store(kestrelConfiguration.Certificate.CertStoreName, kestrelConfiguration.Certificate.CertStoreLocation, OpenFlags.ReadOnly))
                            {
                                var cert = store.Certificates
                                    .FirstOrDefault(x => x.Thumbprint.Equals(kestrelConfiguration.Certificate.Thumbprint, StringComparison.OrdinalIgnoreCase))
                                    ?? throw new Core.Exceptions.CisConfigurationException(0, $"Kestrel certifikate '{kestrelConfiguration.Certificate.Thumbprint}' not found in '{kestrelConfiguration.Certificate.CertStoreName}' / 'kestrelConfiguration.Certificate.CertStoreLocation'");
                                opts.UseHttps(cert);
                            }

                            break;
                    }
                }
            });

            // pridat endpointy kde kestrel bude poslouchat
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
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        return services
            .AddGrpcClient<TService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TService>>();
                options.Address = serviceUri.Url;
            })
            .ConfigureChannel(configureChannel)
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddCallCredentials(addCredentials);
    }

    public static IHttpClientBuilder AddGrpcClientFromCisEnvironment<TService, TServiceUriSettings>(this IServiceCollection services)
        where TService : class 
        where TServiceUriSettings : class
    {
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        return services
            .AddGrpcClient<TService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TServiceUriSettings>>();
                options.Address = serviceUri.Url;
            })
            .ConfigureChannel(configureChannel)
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddCallCredentials(addCredentials);
    }

    public static IServiceCollection AddGrpcServiceUriSettings<TService>(this IServiceCollection services, string serviceUrl)
        where TService : class
    {
        services.TryAddSingleton(new GrpcServiceUriSettings<TService>(serviceUrl));
        return services;
    }

    private static Func<AuthInterceptorContext, Metadata, IServiceProvider, Task> addCredentials =
        (context, metadata, serviceProvider) =>
        {
            var configuration = serviceProvider.GetRequiredService<Core.Configuration.ICisEnvironmentConfiguration>();
            var userAccessor = serviceProvider.GetService<Core.Security.ICurrentUserAccessor>();

            if (string.IsNullOrEmpty(configuration.InternalServicesLogin) || string.IsNullOrEmpty(configuration.InternalServicePassword))
                throw new System.Security.Authentication.InvalidCredentialException("InternalServicesLogin or InternalServicePassword is empty");
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{configuration.InternalServicesLogin}:{configuration.InternalServicePassword}");

            // add authentication header
            metadata.Add("authorization", $"Basic {Convert.ToBase64String(plainTextBytes)}");

            // add context userId
            if (userAccessor is not null && userAccessor.IsAuthenticated)
                metadata.Add(Core.Security.Constants.ContextUserHttpHeaderKey, userAccessor!.User!.Id.ToString(System.Globalization.CultureInfo.InvariantCulture));

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