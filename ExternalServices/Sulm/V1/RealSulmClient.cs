using CIS.Core.Exceptions.ExternalServices;
using System.Net.Http.Json;

namespace ExternalServices.Sulm.V1;

internal sealed class RealSulmClient 
    : ISulmClient
{
    public async Task StopUse(IList<CIS.Foms.Types.UserIdentity> identities, string purposeCode, CancellationToken cancellationToken = default(CancellationToken))
    {
        var identity = getKbIdentity(identities);
        var channgel = ISulmClient.GetChannelCode(identities);

        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/wfs/eventreport/casestatechanged", easRequest, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Contracts.WFS_Event_Response>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CaseStateChanged), nameof(Contracts.WFS_Event_Response));
        }
        else
        {
            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
    }

    private static CIS.Foms.Types.UserIdentity getKbIdentity(IList<CIS.Foms.Types.UserIdentity> identities)
    {
        return identities
            .FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.UserIdentitySchemes.KbUId)
            ?? throw new CisExtServiceValidationException(0, "SULM integration: KB Identity not found");
    }

    private readonly HttpClient _httpClient;
    public RealSulmClient(HttpClient httpClient)
        => _httpClient = httpClient;
}
