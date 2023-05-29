using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MRiskCharacteristics";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IRiskCharacteristicsClientBase
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (RiskCharacteristics.V2.IRiskCharacteristicsClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<RiskCharacteristics.V2.IRiskCharacteristicsClient, RiskCharacteristics.V2.MockRiskCharacteristicsClient>();
                break;

            case (RiskCharacteristics.V2.IRiskCharacteristicsClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<RiskCharacteristics.V2.IRiskCharacteristicsClient, RiskCharacteristics.V2.RealRiskCharacteristicsClient>()
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
            Type t when t.IsAssignableFrom(typeof(RiskCharacteristics.V2.IRiskCharacteristicsClient)) => RiskCharacteristics.V2.IRiskCharacteristicsClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
}
