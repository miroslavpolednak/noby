using CIS.Core.Exceptions.ExternalServices;
using System.Net.Http.Json;

namespace ExternalServices.Sulm.V1;

internal sealed class RealSulmClient 
    : ISulmClient
{
    public async Task StopUse(long partyId, string usageCode, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/eventreport/casestatechanged", easRequest, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Contracts.WFS_Event_Response>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CaseStateChanged), nameof(Contracts.WFS_Event_Response));

            // neco je spatne ve WS
            if ((result.Result?.Return_val ?? 0) != 0)
                throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName}.CaseStateChanged: {result.Result?.Return_text}");
        }
        else
        {
            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
    }

    private readonly HttpClient _httpClient;
    public RealSulmClient(HttpClient httpClient)
        => _httpClient = httpClient;
}
