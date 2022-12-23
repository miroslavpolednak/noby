using DomainServices.CustomerService.ExternalServices.CustomerProfile.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.CustomerProfile.V1;

internal sealed class RealCustomerProfileClient
    : ICustomerProfileClient
{
    public async Task<bool> ValidateProfile(long customerId, string profileCode, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/public/v1/customers/{customerId}/validate-customer-profile?profileCode={profileCode}", cancellationToken)
            .ConfigureAwait(false);

        var result = (await Common.Helpers.ProcessResponse<ValidateCustomerProfileResponse>(StartupExtensions.ServiceName, response, cancellationToken));
        return result.ResultCode == ValidateCustomerProfileResponseResultCode.OK;
    }

    private readonly HttpClient _httpClient;
    public RealCustomerProfileClient(HttpClient httpClient)
        => _httpClient = httpClient;
}