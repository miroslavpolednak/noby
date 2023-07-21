using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "Crem";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, Crem.V1.ICremClient
        => builder.AddESingatures<TClient>(Crem.V1.ICremClient.Version);

    static WebApplicationBuilder AddESingatures<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (Crem.V1.ICremClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<Crem.V1.ICremClient, Crem.V1.MockCremClient>();
                break;

            case (Crem.V1.ICremClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<Crem.V1.ICremClient, Crem.V1.RealCremClient>()
                    .AddExternalServicesCorrelationIdForwarding()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
