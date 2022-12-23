namespace DomainServices.CustomerService.ExternalServices.CustomerProfile.V1;

internal sealed class RealCustomerProfileClient
    : ICustomerProfileClient
{
    public async Task<bool> ValidateProfile(long customerId, string profileCode, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/public/v1/identified-subject" + (hardCreate ? "?hardCreate=true" : ""), request, cancellationToken)
            .ConfigureAwait(false);

        if (response?.IsSuccessStatusCode ?? false)
        {
            return await response.Content.ReadFromJsonAsync<Contracts.CreateIdentifiedSubjectResponse>(cancellationToken: cancellationToken)
                    ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(CreateIdentifiedSubject), nameof(Contracts.CreateIdentifiedSubjectResponse));
        }
        else if (response?.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadFromJsonAsync<Contracts.Error>(cancellationToken: cancellationToken);
            if (error != null)
                throw new CisExtServiceValidationException($"{error.Message}: {error.Detail}");
        }

        throw new CisExtServiceValidationException($"{StartupExtensions.ServiceName} unknown error {response?.StatusCode}: {await response.SafeReadAsStringAsync(cancellationToken)}");



        _logger.LogDebug("Run inputs: CustomerProfile ValidateProfile with customerId {customerId} and profileCode {profileCode}", customerId, profileCode);

        return CallEndpoint(ValidateCustomerProfile);

        async Task<bool> ValidateCustomerProfile()
        {
            var result = await CreateClient().ValidateCustomerProfileAsync(
                customerId: customerId,
                profileCode: profileCode,
                x_B3_TraceId: traceId,
                x_KB_Party_Identity_In_Service: string.Empty,
                x_KB_Orig_System_Identity: CallerSys,
                x_KB_Caller_System_Identity: CallerSys,
                cancellationToken);

            _logger.LogSerializedObject("ValidateCustomerProfileResponse", result);

            return result.ResultCode == ValidateCustomerProfileResponseResultCode.OK;
        }
    }

    private readonly HttpClient _httpClient;
    public RealCustomerProfileClient(HttpClient httpClient)
        => _httpClient = httpClient;
}