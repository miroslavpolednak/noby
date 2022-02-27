using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Security.gRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.Security.InternalServices;

namespace DomainServices.SalesArrangementService.Abstraction;

public static class SalesArrangementServiceExtensions
{
    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, bool isInvalidCertificateAllowed)
    => services
        .AddCisServiceDiscovery(isInvalidCertificateAllowed)
        .registerUriSettings(isInvalidCertificateAllowed)
        .registerServices()
        .registerGrpcServices();

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:SalesArrangementService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(url ?? throw new ArgumentNullException("url", "SalesArrangementService URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddCisContextUser();

        // register storage services
        services.TryAddTransient<ISalesArrangementServiceAbstraction, Services.SalesArrangementService>();
        services.TryAddTransient<ICustomerOnSAServiceAbstraction, Services.CustomerOnSAService>();
        services.TryAddTransient<IHouseholdServiceAbstraction, Services.HouseholdService>();

        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.SalesArrangementService.SalesArrangementServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.HouseholdService.HouseholdServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.HouseholdService.HouseholdServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        return services;
    }
}
