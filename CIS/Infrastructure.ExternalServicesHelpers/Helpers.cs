using CIS.Core.Exceptions.ExternalServices;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class Helpers
{
    public static async Task<string?> SafeReadAsStringAsync(this HttpResponseMessage? response, CancellationToken cancellationToken = default(CancellationToken))
        => response?.Content == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

    public static async Task EnsureSuccessStatusCode(this HttpResponseMessage? response, string serviceName, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (!response?.IsSuccessStatusCode ?? true)
            throw new CisExtServiceValidationException($"{serviceName} unknown error {response!.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
    }
}
