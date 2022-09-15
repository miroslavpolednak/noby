using CIS.Infrastructure.Logging;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

internal class RealCustomerProfileClient : BaseClient<ApiException<Error>>, ICustomerProfileClient
{
    private const string CallerSys = "{\"app\":\"DOMAIN_SERVICES\",\"appComp\":\"DOMAIN_SERVICES.CUSTOMER_SERVICE\"}";

    public RealCustomerProfileClient(HttpClient httpClient, ILogger<RealCustomerProfileClient> logger) : base(httpClient, logger)
    {
    }

    public Task<bool> ValidateProfile(long customerId, string profileCode, string traceId, CancellationToken cancellationToken)
    {
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

    protected CustomerProfileWrapper CreateClient() => new(_httpClient.BaseAddress?.ToString(), _httpClient);

    protected override int GetApiExceptionStatusCode(ApiException<Error> ex) => ex.StatusCode;

    protected override object GetApiExceptionDetail(ApiException<Error> ex) => ex.Result.Detail;
}