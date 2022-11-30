using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.SbWebApi;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static IHttpClientBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class
    {
        return typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.ISbWebApiClient)) 
                => builder
                    .AddExternalServiceRestClient<V1.ISbWebApiClient, V1.RealSbWebApiClient, ExternalServiceConfiguration<V1.ISbWebApiClient>>(ServiceName, "V1", _addAdditionalHttpHandlers),
            _ => throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented")
        };
    }

    private static Action<IHttpClientBuilder, ExternalServiceConfiguration<V1.ISbWebApiClient>> _addAdditionalHttpHandlers = (builder, configuration)
        => builder
            .AddExternalServicesCorrelationIdForwarding()
            .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
}
