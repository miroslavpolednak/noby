using CIS.DomainServicesSecurity.Abstraction;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.CaseService.Abstraction;

public static class CaseServiceExtensions
{
    public static IServiceCollection AddCaseService(this IServiceCollection services, bool isInvalidCertificateAllowed)
    => services
        .AddCisServiceDiscovery(isInvalidCertificateAllowed)
        .registerUriSettings(isInvalidCertificateAllowed)
        .registerServices()
        .registerGrpcServices();

    public static IServiceCollection AddCaseService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.CaseService.CaseServiceClient>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.CaseService.CaseServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:CaseService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.v1.CaseService.CaseServiceClient>(url ?? throw new ArgumentNullException("url", "CaseService URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddCisUserContextHelpers();

        // register service
        services.TryAddTransient<ICaseServiceAbstraction, Services.CaseService>();

        // exception handling
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddSingleton<ExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.CaseService.CaseServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.CaseService.CaseServiceClient>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.v1.CaseService.CaseServiceClient>()
                .AddInterceptor<GenericClientExceptionInterceptor>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        return services;
    }
}
