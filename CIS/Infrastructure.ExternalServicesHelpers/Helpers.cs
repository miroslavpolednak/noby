using CIS.Core.Exceptions.ExternalServices;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class Helpers
{
    public static async Task<string?> SafeReadAsStringAsync(this HttpResponseMessage? response, CancellationToken cancellationToken = default)
        => response?.Content == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

    public static async Task EnsureSuccessStatusCode(this HttpResponseMessage? response, string serviceName, CancellationToken cancellationToken = default)
    {
        if (!response?.IsSuccessStatusCode ?? true)
            throw new CisExtServiceValidationException($"{serviceName} unknown error {response!.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
    }

    public static async Task<TResponse> EnsureSuccessStatusAndReadJson<TResponse>(this HttpResponseMessage? response, string serviceName, CancellationToken cancellationToken = default, [CallerMemberName] string callerName = "")
        where TResponse : class, new()
    {
        await response.EnsureSuccessStatusCode(serviceName, cancellationToken);

        return await response!.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, serviceName, callerName, typeof(TResponse).ToString());
    }
}
