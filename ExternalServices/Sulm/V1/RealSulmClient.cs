using CIS.Core;
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
        var request = createRequest(kbCustomerId, userIdentities, purposeCode);

        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _apiBasePath + "start", request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await processUnsuccessfulResult(response, cancellationToken);
        }
    }

    public async Task StopUse(
        long kbCustomerId, 
        IList<CIS.Foms.Types.UserIdentity> userIdentities, 
        string purposeCode, 
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = createRequest(kbCustomerId, userIdentities, purposeCode);
        
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _apiBasePath + "stop", request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await processUnsuccessfulResult(response, cancellationToken);
        }
    }

    private static Dto.StartUseRequest createRequest(
        long kbCustomerId,
        IList<CIS.Foms.Types.UserIdentity> userIdentities,
        string purposeCode)
    {
        var identity = getKbIdentity(userIdentities);

        return new Dto.StartUseRequest
        {
            channelCode = ISulmClient.GetChannelCode(userIdentities),
            clientId = kbCustomerId.ToString(),
            userIdType = identity.Scheme.GetAttribute<DisplayAttribute>()!.Name!,
            userId = identity.Identity,
            purposeCode = purposeCode
        };
    }

    private async Task processUnsuccessfulResult(HttpResponseMessage response, CancellationToken cancellationToken)
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

    private static CIS.Foms.Types.UserIdentity getKbIdentity(IList<CIS.Foms.Types.UserIdentity> identities)
    {
        return identities
            .FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.UserIdentitySchemes.KbUid)
            ?? throw new CisExtServiceValidationException(0, "SULM integration: KB Identity not found");
    }

    private const string _apiBasePath = "api/customers/sulm/v1/use/";

    private readonly HttpClient _httpClient;
    public RealSulmClient(HttpClient httpClient)
        => _httpClient = httpClient;
}
