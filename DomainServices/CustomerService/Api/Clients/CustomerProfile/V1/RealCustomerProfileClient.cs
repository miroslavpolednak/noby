using CIS.Infrastructure.Logging;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

public class RealCustomerProfileClient : BaseClient, ICustomerProfileClient
{
    private const string CallerSys = "{\"app\":\"DOMAIN_SERVICES\",\"appComp\":\"DOMAIN_SERVICES.CUSTOMER_SERVICE\"}";

    public RealCustomerProfileClient(HttpClient httpClient, ILogger<RealCustomerProfileClient> logger) : base(httpClient, logger)
    {
    }

    public Task<bool> ValidateProfile(long customerId, string profileCode, string traceId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Run inputs: CustomerProfile ValidateProfile with customerId {customerId} and profileCode {profileCode}", customerId, profileCode);

        return CallEndpoint(ValidateCustomerProfile);

        async Task<bool> ValidateCustomerProfile(CustomerProfileWrapper client)
        {
            var result = await client.ValidateCustomerProfileAsync(
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
}