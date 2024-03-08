using CIS.Core.Exceptions.ExternalServices;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class Helpers
{
    private static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static async Task<string?> SafeReadAsStringAsync(this HttpResponseMessage? response, CancellationToken cancellationToken = default)
        => response?.Content == null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

    public static async Task EnsureSuccessStatusCodeWithCustomErrorCodes(
        this HttpResponseMessage? response,
        string serviceName,
        Dictionary<System.Net.HttpStatusCode, int>? customErrorCodes,
        CancellationToken cancellationToken = default)
    {
        if (!response?.IsSuccessStatusCode ?? true)
        {
            if (customErrorCodes?.ContainsKey(response!.StatusCode) ?? false)
            {
                throw new CisExternalServiceValidationException(customErrorCodes[response.StatusCode], await response.SafeReadAsStringAsync(cancellationToken) ?? "Empty error message");
            }
            else
            {
                switch (response!.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        throw new CisExternalServiceValidationException($"{serviceName} Not found: {await response.SafeReadAsStringAsync(cancellationToken)}");
                        
                    case System.Net.HttpStatusCode.BadRequest:
                        throw new CisExternalServiceValidationException($"{serviceName} Bad request: {await response.SafeReadAsStringAsync(cancellationToken)}");

                    default:
                        throw new CisExternalServiceValidationException($"{serviceName} unknown error {response!.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
                }
            }
        }
    }

    public static async Task EnsureSuccessStatusCode(
        this HttpResponseMessage? response,
        string serviceName,
        CancellationToken cancellationToken = default)
        => await EnsureSuccessStatusCodeWithCustomErrorCodes(response, serviceName, null, cancellationToken);

    public static async Task<TResponse> EnsureSuccessStatusAndReadJson<TResponse>(
        this HttpResponseMessage? response, 
        string serviceName, 
        CancellationToken cancellationToken = default,
        [CallerMemberName] string callerName = "")
        where TResponse : class, new()
    {
        await response.EnsureSuccessStatusCode(serviceName, cancellationToken: cancellationToken);

        return await response!.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions, cancellationToken)
            ?? throw new CisExternalServiceResponseDeserializationException(0, serviceName, callerName, typeof(TResponse).ToString());
    }

    public static async Task<TResponse> EnsureSuccessStatusAndReadJson<TResponse>(
        this HttpResponseMessage? response,
        string serviceName,
        Dictionary<System.Net.HttpStatusCode, int> customErrorCodes,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string callerName = "")
        where TResponse : class, new()
    {
        await response.EnsureSuccessStatusCodeWithCustomErrorCodes(serviceName, customErrorCodes, cancellationToken);

        return await response!.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions, cancellationToken)
            ?? throw new CisExternalServiceResponseDeserializationException(0, serviceName, callerName, typeof(TResponse).ToString());
    }
}
