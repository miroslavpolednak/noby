using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "ESignatures";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ESignatures.V1.IESignaturesClient
        => builder.AddESingatures<TClient>();

    static WebApplicationBuilder AddESingatures<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (ESignatures.V1.IESignaturesClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<ESignatures.V1.IESignaturesClient, ESignatures.V1.MockESignaturesClient>();
                break;

            case (ESignatures.V1.IESignaturesClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<ESignatures.V1.IESignaturesClient, ESignatures.V1.RealESignaturesClient>()
                    .AddExternalServicesCorrelationIdForwarding()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(ESignatures.V1.IESignaturesClient)) => ESignatures.V1.IESignaturesClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };
}
