﻿using CIS.Core;
using ExternalServices.Sulm.V1.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ExternalServices.Sulm.V1;

internal sealed class RealSulmClient 
    : ISulmClient
{
    public async Task StartUse(
        long kbCustomerId,
        IList<SharedTypes.Types.UserIdentity> userIdentities,
        string purposeCode,
        CancellationToken cancellationToken = default)
    {
        var request = createRegisterRequest(kbCustomerId, userIdentities, purposeCode);

        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _apiBasePath + "register", request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await processUnsuccessfulResult(response, cancellationToken);
        }
    }

    public async Task StopUse(
        long kbCustomerId, 
        IList<SharedTypes.Types.UserIdentity> userIdentities, 
        string purposeCode, 
        CancellationToken cancellationToken = default)
    {
        var request = terminateRegisterRequest(kbCustomerId, userIdentities, purposeCode);
        
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _apiBasePath + "terminate", request, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await processUnsuccessfulResult(response, cancellationToken);
        }
    }

    private static RegisterClientPurposeRequest createRegisterRequest(
        long kbCustomerId,
        IList<SharedTypes.Types.UserIdentity> userIdentities,
        string purposeCode)
    {
        var identity = getKbIdentity(userIdentities);

        return new RegisterClientPurposeRequest
        {
            ChannelCode = ISulmClient.GetChannelCode(userIdentities),
            ClientId = kbCustomerId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            PurposeCode = purposeCode,
            UserId = identity.Identity,
            UserIdType = identity.Scheme.GetAttribute<DisplayAttribute>()!.Name!
        };
    }

    private static TerminateClientPurposeRequest terminateRegisterRequest(
        long kbCustomerId,
        IList<SharedTypes.Types.UserIdentity> userIdentities,
        string purposeCode)
    {
        var identity = getKbIdentity(userIdentities);

        return new TerminateClientPurposeRequest
        {
            ChannelCode = ISulmClient.GetChannelCode(userIdentities),
            ClientId = kbCustomerId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            PurposeCode = purposeCode,
            UserId = identity.Identity,
            UserIdType = identity.Scheme.GetAttribute<DisplayAttribute>()!.Name!
        };
    }

    private static async Task processUnsuccessfulResult(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var result = await response.Content.ReadFromJsonAsync<Error>(cancellationToken: cancellationToken);
        if (result is null)
        {
            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
        else
        {
            throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} error {response.StatusCode}: {result.Code}");
        }
    }

    private static SharedTypes.Types.UserIdentity getKbIdentity(IList<SharedTypes.Types.UserIdentity> identities)
    {
        return identities
            .FirstOrDefault(t => _allowedIdentities.Contains(t.Scheme))
            ?? throw new CisExtServiceValidationException(0, "SULM integration: User does not have supported identity");
    }

    private static SharedTypes.Enums.UserIdentitySchemes[] _allowedIdentities = new[]
    {
        SharedTypes.Enums.UserIdentitySchemes.KbUid,
        SharedTypes.Enums.UserIdentitySchemes.BrokerId,
        SharedTypes.Enums.UserIdentitySchemes.Mpad
    };
    private const string _apiBasePath = "api/customers/sulm/v1/client/purpose/";

    private readonly HttpClient _httpClient;
    public RealSulmClient(HttpClient httpClient)
        => _httpClient = httpClient;
}
