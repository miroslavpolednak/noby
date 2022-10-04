using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using _Clients = DomainServices.RiskIntegrationService.Clients;
using _Contracts = DomainServices.RiskIntegrationService.Contracts;

namespace DomainServices.RiskIntegrationService.Clients;

public static class RiskIntegrationServiceExtensions
{
    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services)
        => services
            .TryAddGrpcClient<_Contracts.CreditWorthiness.V2.ICreditWorthinessService>(a =>
                a.AddGrpcServiceUriSettingsFromServiceDiscovery<_Contracts.IGrpcSettingsMarker>("DS:RiskIntegrationService")
            .registerServices()
        );

    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, string serviceUrl)
        => services
            .TryAddGrpcClient<_Contracts.CreditWorthiness.V2.ICreditWorthinessService>(a =>
                a.AddGrpcServiceUriSettings<_Contracts.IGrpcSettingsMarker>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();

        services.register<_Contracts.CreditWorthiness.V2.ICreditWorthinessService, _Clients.CreditWorthiness.V2.ICreditWorthinessServiceClient, Services.CreditWorthiness.V2.CreditWorthinessService>();

        services.register<_Contracts.CustomersExposure.V2.ICustomersExposureService, _Clients.CustomersExposure.V2.ICustomersExposureServiceClient, Services.CustomersExposure.V2.CustomersExposureService>();

        services.register<_Contracts.LoanApplication.V2.ILoanApplicationService, _Clients.LoanApplication.V2.ILoanApplicationServiceClient, Services.LoanApplication.V2.LoanApplicationService>();

        services.register<_Contracts.RiskBusinessCase.V2.IRiskBusinessCaseService, _Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient, Services.RiskBusinessCase.V2.RiskBusinessCaseService>();

        return services;
    }

    static void register<IService, IAbstraction, TAbstraction>(this IServiceCollection services)
        where IService : class
        where IAbstraction : class
        where TAbstraction : class
        => services
            .AddTransient(typeof(IAbstraction), typeof(TAbstraction))
            .AddCodeFirstGrpcClient<IService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<_Contracts.IGrpcSettingsMarker>>();
                options.Address = serviceUri.Url;
            })
            .CisConfigureChannel()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();
}
