using Microsoft.AspNetCore.Builder;
using CIS.ExternalServicesHelpers;
using CIS.ExternalServicesHelpers.Configuration;

namespace ExternalServices.SbWebApi;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static WebApplicationBuilder AddExternalServiceSbWebApi<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ISbWebApi
    {
        var httpClientBuilder = typeof(TClient) switch
        {
            V1.ISbWebApiClient => builder.AddExternalServiceClient<V1.ISbWebApiClient, V1.RealSbWebApiClient, ExternalServiceConfiguration<V1.ISbWebApiClient>>(ServiceName, "V1"),
            _ => throw new NotImplementedException($"{ServiceName} version V1 client not implemented")
        };

        return builder;
    }

    
}
