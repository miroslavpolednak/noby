using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase;
using DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V1.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MRiskBusinessCase";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IRiskBusinessCaseClientBase
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (RiskBusinessCase.V1.IRiskBusinessCaseClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<RiskBusinessCase.V1.IRiskBusinessCaseClient, RiskBusinessCase.V1.MockRiskBusinessCaseClient>();
                break;

            case (RiskBusinessCase.V1.IRiskBusinessCaseClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<RiskBusinessCase.V1.IRiskBusinessCaseClient, RiskBusinessCase.V1.RealRiskBusinessCaseClient>()
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
            Type t when t.IsAssignableFrom(typeof(RiskBusinessCase.V1.IRiskBusinessCaseClient)) => RiskBusinessCase.V1.IRiskBusinessCaseClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };

    private static IHttpClientBuilder AddBadRequestHandling(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler(b =>
        {
            return new BadRequestHttpHandler(StartupExtensions.ServiceName);
        });
}
