using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.RealEstateValuationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "PreorderService";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, PreorderService.V1.IPreorderServiceClient
        => builder.AddPreorderService<TClient>(PreorderService.V1.IPreorderServiceClient.Version);

    static WebApplicationBuilder AddPreorderService<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (PreorderService.V1.IPreorderServiceClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<PreorderService.V1.IPreorderServiceClient, PreorderService.V1.MockPreorderServiceClient>();
                break;

            case (PreorderService.V1.IPreorderServiceClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<PreorderService.V1.IPreorderServiceClient, PreorderService.V1.RealPreorderServiceClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        PreorderService.ErrorCodeMapper.Init();

        return builder;
    }
}
