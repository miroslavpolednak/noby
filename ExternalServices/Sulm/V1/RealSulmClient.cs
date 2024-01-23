using CIS.Core;
using CIS.Core.Configuration;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using ExternalServices.Sulm.V1.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ExternalServices.Sulm.V1;

internal sealed class RealSulmClient 
    : ISulmClient
{
    public async Task StartUse(
        long kbCustomerId,
        string purposeCode,
        IList<SharedTypes.Types.UserIdentity>? userIdentities,
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
        string purposeCode,
        IList<SharedTypes.Types.UserIdentity>? userIdentities, 
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

    private RegisterClientPurposeRequest createRegisterRequest(
        long kbCustomerId,
        IList<SharedTypes.Types.UserIdentity>? userIdentities,
        string purposeCode)
    {
        var identity = getCallingIdentity(userIdentities);

        return new RegisterClientPurposeRequest
        {
            ChannelCode = ISulmClient.GetChannelCode(userIdentities),
            ClientId = kbCustomerId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            PurposeCode = purposeCode,
            UserId = identity.Identity,
            UserIdType = identity.Scheme.GetAttribute<DisplayAttribute>()!.Name!
        };
    }

    private TerminateClientPurposeRequest terminateRegisterRequest(
        long kbCustomerId,
        IList<SharedTypes.Types.UserIdentity>? userIdentities,
        string purposeCode)
    {
        var identity = getCallingIdentity(userIdentities);

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
            throw new CisExternalServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");
        }
        else
        {
            throw new CisExternalServiceValidationException($"{StartupExtensions.ServiceName} error {response.StatusCode}: {result.Code}");
        }
    }

    private SharedTypes.Types.UserIdentity getCallingIdentity(IList<SharedTypes.Types.UserIdentity>? identities)
    {
        var identity = identities?.FirstOrDefault(t => _allowedIdentities.Contains(t.Scheme));
        if (identity is null)
        {
            return new SharedTypes.Types.UserIdentity
            {
                Scheme = SharedTypes.Enums.UserIdentitySchemes.KbUms,
                Identity = _configuration.Username ?? ""
            };
        }
        return identity;
    }

    private static SharedTypes.Enums.UserIdentitySchemes[] _allowedIdentities = new[]
    {
        SharedTypes.Enums.UserIdentitySchemes.KbUid,
        SharedTypes.Enums.UserIdentitySchemes.BrokerId,
        SharedTypes.Enums.UserIdentitySchemes.Mpad
    };
    private const string _apiBasePath = "api/customers/sulm/v1/client/purpose/";

    private readonly HttpClient _httpClient;
    private readonly IExternalServiceConfiguration<ISulmClient> _configuration;

    public RealSulmClient(HttpClient httpClient, IExternalServiceConfiguration<ISulmClient> configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
}
