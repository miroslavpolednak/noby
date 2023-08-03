using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;
using CIS.Core.Exceptions;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;

namespace DomainServices.RealEstateValuationService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "LuxpiService";
    
    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, LuxpiService.V1.ILuxpiServiceClient
        => builder.AddLuxpiService<TClient>(LuxpiService.V1.ILuxpiServiceClient.Version);

    static WebApplicationBuilder AddLuxpiService<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        if (string.IsNullOrEmpty(configuration.Password))
        {
            throw new CisConfigurationException(0, "Luxpi API key not found");
        }

        switch (version, configuration.ImplementationType)
        {
            case (LuxpiService.V1.ILuxpiServiceClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<LuxpiService.V1.ILuxpiServiceClient, LuxpiService.V1.MockLuxpiServiceClient>();
                break;

            case (LuxpiService.V1.ILuxpiServiceClient.Version, ServiceImplementationTypes.Real):
                // potrebujeme dalsiho httpclienta kvuli ziskane bearer tokenu pro hlavni sluzbu
                builder
                    .AddExternalServiceRestClient<LuxpiService.TokenService.ITokenService, LuxpiService.TokenService.RealTokenService, IExternalServiceConfiguration<LuxpiService.V1.ILuxpiServiceClient>>()
                    .AddExternalServicesErrorHandling("LuxpiTokenService");

                builder
                    .AddExternalServiceRestClient<LuxpiService.V1.ILuxpiServiceClient, LuxpiService.V1.RealLuxpiServiceClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesKbPartyHeaders()
                    // propagace bearer tokenu
                    .AddHttpMessageHandler(services => new LuxpiService.TokenService.TokenHttpHandler(configuration.Password, services.GetRequiredService<LuxpiService.TokenService.ITokenService>()))
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        LuxpiService.ErrorCodeMapper.Init();

        return builder;
    }
}