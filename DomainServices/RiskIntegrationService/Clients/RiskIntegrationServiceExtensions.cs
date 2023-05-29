using CIS.Core;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.ClientFactory;
using System.Configuration;
using __Clients = DomainServices.RiskIntegrationService.Clients;
using __Contracts = DomainServices.RiskIntegrationService.Contracts;

namespace DomainServices;

public static class RiskIntegrationServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:RiskIntegrationService";

    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.IGrpcSettingsMarker>>(new GrpcServiceUriSettingsServiceDiscovery<__Contracts.IGrpcSettingsMarker>(ServiceName));
        services.registerServices();
        return services;
    }

    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.IGrpcSettingsMarker>>(new GrpcServiceUriSettingsDirect<__Contracts.IGrpcSettingsMarker>(serviceUrl));
        services.registerServices();
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddScoped<ContextUserForwardingClientInterceptor>();

        services.register<__Contracts.CreditWorthiness.V2.ICreditWorthinessService, __Clients.CreditWorthiness.V2.ICreditWorthinessServiceClient, __Clients.Services.CreditWorthiness.V2.CreditWorthinessService>();

        services.register<__Contracts.CustomerExposure.V2.ICustomerExposureService, __Clients.CustomerExposure.V2.ICustomerExposureServiceClient, __Clients.Services.CustomersExposure.V2.CustomerExposureService>();

        services.register<__Contracts.LoanApplication.V2.ILoanApplicationService, __Clients.LoanApplication.V2.ILoanApplicationServiceClient, __Clients.Services.LoanApplication.V2.LoanApplicationService>();

        services.register<__Contracts.RiskBusinessCase.V2.IRiskBusinessCaseService, __Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient, __Clients.Services.RiskBusinessCase.V2.RiskBusinessCaseService>();

        return services;
    }

    static void register<IService, IClient, TClient>(this IServiceCollection services, bool validateServiceCertificate = false)
        where IService : class
        where IClient : class
        where TClient : class
    {
        if (services.AlreadyRegistered<IClient>())
            return;

        var builder = services
            .AddTransient(typeof(IClient), typeof(TClient))
            .AddCodeFirstGrpcClient<IService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<IGrpcServiceUriSettings<__Contracts.IGrpcSettingsMarker>>();
                options.Address = serviceUri.ServiceUrl;
            })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();

        if (!validateServiceCertificate)
            builder.CisConfigureChannelWithoutCertificateValidation();
    }
}
