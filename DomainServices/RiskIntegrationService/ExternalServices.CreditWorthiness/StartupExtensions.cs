using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MCreditWorthiness";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ICreditWorthinessClientBase
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (CreditWorthiness.V3.ICreditWorthinessClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<CreditWorthiness.V3.ICreditWorthinessClient, CreditWorthiness.V3.MockCreditWorthinessClient>();
                break;

            case (CreditWorthiness.V3.ICreditWorthinessClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<CreditWorthiness.V3.ICreditWorthinessClient, CreditWorthiness.V3.RealCreditWorthinessClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName)
                    .AddBadRequestHandling();
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(CreditWorthiness.V3.ICreditWorthinessClient)) => CreditWorthiness.V3.ICreditWorthinessClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
}
