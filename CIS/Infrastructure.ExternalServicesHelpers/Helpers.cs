using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.AspNetCore.Builder;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class Helpers
{
    public static async Task<string?> SafeReadAsStringAsync(this HttpResponseMessage? response, CancellationToken cancellationToken = default(CancellationToken))
        => response?.Content == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

    public static ExternalServiceConfiguration<TClient> GetExternalServiceBaseConfiguration<TClient>(this WebApplicationBuilder builder, string serviceName)
        where TClient : class, IExternalServiceClient
    {
        // get version from interface
        var serviceImplementationVersion = ((IExternalServiceClient)typeof(TClient)).GetVersion();
        
        return builder.AddExternalServiceConfiguration<TClient, ExternalServiceConfiguration<TClient>>(serviceName, serviceImplementationVersion);
    }
}
