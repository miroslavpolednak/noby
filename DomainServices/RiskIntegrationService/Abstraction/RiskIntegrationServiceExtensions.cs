using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;

namespace DomainServices.RiskIntegrationService.Abstraction;

public static class RiskIntegrationServiceExtensions
{
    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services)
        => services
            .TryAddGrpcClient<CreditWorthiness.V2.ICreditWorthinessService>(a =>
                a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.IGrpcSettingsMarker>("DS:RiskIntegrationService")
            .registerServices()
        );

    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, string serviceUrl)
        => services
            .TryAddGrpcClient<CreditWorthiness.V2.ICreditWorthinessService>(a =>
                a.AddGrpcServiceUriSettings<Contracts.IGrpcSettingsMarker>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.AddTransient<CreditWorthiness.V2.ICreditWorthinessService, Services.CreditWorthiness.V2.CreditWorthinessService>();
        services.AddTransient<CustomersExposure.V2.ICustomersExposureService, Services.CustomersExposure.V2.CustomersExposureService>();
        services.AddTransient<LoanApplication.V2.ILoanApplicationService, Services.LoanApplication.V2.LoanApplicationService>();

        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();

        services
            .AddCodeFirstGrpcClient<CreditWorthiness.V2.ICreditWorthinessService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<Contracts.IGrpcSettingsMarker>>();
                options.Address = serviceUri.Url;
            })
            .CisConfigureChannel()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();

        return services;
    }
}
