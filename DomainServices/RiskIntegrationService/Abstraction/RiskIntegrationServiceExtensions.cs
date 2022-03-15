using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Security.gRPC;
using CIS.Security.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.RiskIntegrationService.Abstraction;

public static class RiskIntegrationServiceExtensions
{
    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, bool isInvalidCertificateAllowed)
    => services
        .AddCisServiceDiscovery(isInvalidCertificateAllowed)
        .registerUriSettings(isInvalidCertificateAllowed)
        .registerServices()
        .registerGrpcServices();

    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:CaseService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient>(url ?? throw new ArgumentNullException("url", "CaseService URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddCisContextUser();

        // register storage services
        services.TryAddTransient<IRiskIntegrationServiceAbstraction, Services.RiskIntegrationService>();

        // exception handling
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient>()
                .AddInterceptor<GenericClientExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        return services;
    }
}
