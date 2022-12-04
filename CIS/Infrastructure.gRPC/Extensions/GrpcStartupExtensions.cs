using CIS.Infrastructure.gRPC.Configuration;
using Grpc.Core;
using Grpc.Net.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Extension metody do startupu aplikace pro registraci gRPC služeb.
/// </summary>
public static class GrpcStartupExtensions
{
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
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(Validation.GrpcValidationBehaviour<,>));

        // add validators
        services.Scan(selector => selector
                .FromAssembliesOf(assemblyType)
                .AddClasses(x => x.AssignableTo(typeof(FluentValidation.IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return services;
    }

    public static IApplicationBuilder UseGrpc2WebApiException(this IApplicationBuilder app)
        => app.UseWhen(context => context.Request.ContentType == "application/json", builder => app.UseMiddleware<Middleware.Grpc2WebApiExceptionMiddleware>());

    public static IServiceCollection TryAddGrpcClient<TService>(this IServiceCollection services, Action<IServiceCollection> registration)
    {
        if (!services.Any(t => t.ServiceType == typeof(TService)))
        {
            registration(services);
        }
        return services;
    }

    public static IHttpClientBuilder AddGrpcClientFromCisEnvironment<TService>(this IServiceCollection services, bool validateServiceCertificate = false) 
        where TService : class
    {
        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();
        
        var builder = services
            .AddGrpcClient<TService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<IGrpcServiceUriSettings<TService>>();
                options.Address = serviceUri.ServiceUrl;
            })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();

        if (!validateServiceCertificate)
            builder.CisConfigureChannelWithoutCertificateValidation();

        return builder;
    }

    public static IHttpClientBuilder AddGrpcClientFromCisEnvironment<TService, TServiceUriSettings>(this IServiceCollection services, bool validateServiceCertificate = false)
        where TService : class 
        where TServiceUriSettings : class
    {
        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();

        var builder = services
            .AddGrpcClient<TService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<IGrpcServiceUriSettings<TServiceUriSettings>>();
                options.Address = serviceUri.ServiceUrl;
            })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();

        if (!validateServiceCertificate)
            builder.CisConfigureChannelWithoutCertificateValidation();

        return builder;
    }

    public static IServiceCollection AddGrpcServiceUriSettings<TService>(this IServiceCollection services, string serviceUrl)
        where TService : class
    {
        services.TryAddSingleton(new Configuration.GrpcServiceUriSettingsDirect<TService>(serviceUrl));
        return services;
    }

    public static IHttpClientBuilder CisConfigureChannelWithoutCertificateValidation(this IHttpClientBuilder builder)
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