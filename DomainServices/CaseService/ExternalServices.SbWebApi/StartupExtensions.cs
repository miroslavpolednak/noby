using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.CaseService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, SbWebApi.V1.ISbWebApiClient
        => builder.AddSbWebApi<TClient>(SbWebApi.V1.ISbWebApiClient.Version);

    static WebApplicationBuilder AddSbWebApi<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (SbWebApi.V1.ISbWebApiClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<SbWebApi.V1.ISbWebApiClient, SbWebApi.V1.MockSbWebApiClient>();
                break;

            case (SbWebApi.V1.ISbWebApiClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<SbWebApi.V1.ISbWebApiClient, SbWebApi.V1.RealSbWebApiClient>()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
