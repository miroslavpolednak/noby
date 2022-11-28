using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;

namespace ExternalServices.SbWebApi;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ISbWebApi
    {
        var httpClientBuilder = typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.ISbWebApiClient)) => builder.AddExternalServiceClient<V1.ISbWebApiClient, V1.RealSbWebApiClient, ExternalServiceBasicAuthenticationConfiguration<V1.ISbWebApiClient>>(ServiceName, "V1"),
            _ => throw new NotImplementedException($"{ServiceName} version V1 client not implemented")
        };

        return builder;
    }
}
