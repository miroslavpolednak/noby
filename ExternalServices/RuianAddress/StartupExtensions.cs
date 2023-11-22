using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "RuianAddress";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, RuianAddress.V1.IRuianAddressClient
        => builder.AddESingatures<TClient>(RuianAddress.V1.IRuianAddressClient.Version);

    static WebApplicationBuilder AddESingatures<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (RuianAddress.V1.IRuianAddressClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<RuianAddress.V1.IRuianAddressClient, RuianAddress.V1.MockRuianAddressClient>();
                break;

            case (RuianAddress.V1.IRuianAddressClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<RuianAddress.V1.IRuianAddressClient, RuianAddress.V1.RealRuianAddressClient>()
                    .AddExternalServicesCorrelationIdForwarding()
                    .AddExternalServicesErrorHandling(ServiceName)
                    .ConfigureHttpMessageHandlerBuilder(builder =>
                    {
                        var clientHandler = builder.PrimaryHandler as HttpClientHandler;

                        if (builder.PrimaryHandler is DelegatingHandler delegatingHandler)
                            clientHandler = delegatingHandler.InnerHandler as HttpClientHandler;

                        if (clientHandler is null)
                            return;

                        clientHandler.UseProxy = false;
                    });
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}