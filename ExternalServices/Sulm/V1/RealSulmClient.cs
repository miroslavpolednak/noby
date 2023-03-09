using CIS.Core;
using CIS.Core.Exceptions.ExternalServices;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ExternalServices.Sulm.V1;

internal sealed class RealSulmClient 
    : ISulmClient
{
    public async Task StartUse(
        long kbCustomerId,
        IList<CIS.Foms.Types.UserIdentity> userIdentities,
        string purposeCode,
        CancellationToken cancellationToken = default(CancellationToken))
    {
    }

    public async Task StopUse(
        long kbCustomerId, 
        IList<CIS.Foms.Types.UserIdentity> userIdentities, 
        string purposeCode, 
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var identity = getKbIdentity(userIdentities);
        var request = new Dto.StartUseRequest
        {
            channelCode = ISulmClient.GetChannelCode(userIdentities),
            clientId = kbCustomerId.ToString(),
            userIdType = identity.Scheme.GetAttribute<DisplayAttribute>()!.Name!,
            userId = identity.Identity,
            purposeCode = purposeCode
        };
        
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/customers/sulm/v1/use/start", request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Dto.Error>(cancellationToken: cancellationToken);
            if (result is null)
            {
                throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
            }
            else
            {
                throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} error {response.StatusCode}: {result.code}");
            }
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
