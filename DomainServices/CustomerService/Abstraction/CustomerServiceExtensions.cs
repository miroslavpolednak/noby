using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Security.gRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.Security.InternalServices;

namespace DomainServices.CustomerService.Abstraction;

public static class CustomerServiceExtensions
{
    /// <summary>
    /// Override for integration testing
    /// </summary>
    internal static IServiceCollection AddCustomerServiceTest(this IServiceCollection services, Action<IServiceProvider, Grpc.Net.ClientFactory.GrpcClientFactoryOptions> customConfiguration)
    {
        services
           .AddGrpcClient<Contracts.V1.CustomerService.CustomerServiceClient>(customConfiguration)
           .AddInterceptor<ExceptionInterceptor>()
           .AddInterceptor<AuthenticationInterceptor>();

        return services.registerServices();
    }

    public static IServiceCollection AddCustomerService(this IServiceCollection services, bool isInvalidCertificateAllowed)
        => services
            .AddCisServiceDiscovery(isInvalidCertificateAllowed)
            .registerUriSettings(isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddCustomerService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:CustomerService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>(url ?? throw new ArgumentNullException("url", "CustomerService URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddCisContextUser();

        // register storage services
        services.TryAddTransient<ICustomerServiceAbstraction, CustomerService>();

        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.V1.CustomerService.CustomerServiceClient)))
        {
            services
            .AddGrpcClientFromCisEnvironment<Contracts.V1.CustomerService.CustomerServiceClient>()
            .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.V1.CustomerService.CustomerServiceClient>()
            .AddInterceptor<ExceptionInterceptor>()
            .AddInterceptor<AuthenticationInterceptor>();
        }
        return services;
    }
}
