using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Security.gRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.Security.InternalServices;

namespace DomainServices.UserService.Abstraction;

public static class UserServiceExtensions
{
    public static IServiceCollection AddUserService(this IServiceCollection services, bool isInvalidCertificateAllowed)
    => services
        .AddCisServiceDiscovery(isInvalidCertificateAllowed)
        .registerUriSettings(isInvalidCertificateAllowed)
        .registerServices()
        .registerGrpcServices();

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.UserService.UserServiceClient>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.UserService.UserServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:UserService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.v1.UserService.UserServiceClient>(url ?? throw new ArgumentNullException("url", "UserService URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddCisContextUser();

        // register storage services
        services.TryAddTransient<IUserServiceAbstraction, Services.UserService>();

        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.UserService.UserServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.UserService.UserServiceClient, Contracts.v1.UserService.UserServiceClient>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.v1.UserService.UserServiceClient>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        return services;
    }
}
