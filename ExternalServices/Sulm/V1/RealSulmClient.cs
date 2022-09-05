using CIS.Infrastructure.Logging;
using ExternalServices.Sulm.V1.SulmWrapper;
using System.Diagnostics;

namespace ExternalServices.Sulm.V1;

internal sealed class RealSulmClient 
    : Shared.BaseClient<RealSulmClient>, ISulmClient
{
    public async Task<IServiceCallResult> StopUse(long partyId, string usageCode)
    {
        return await callMethod(async () =>
        {
            using SulmServiceClient client = createClient();

            var request = new stopUseRequest
            {
                partyId = partyId,
                usageCode = usageCode
            };
            _logger.LogSerializedObject("StopUseRequest", request);
            await client.stopUseAsync(getCallerContext(), getCorrelationContext(), request);

            return new SuccessfulServiceCallResult();
        });
    }

    public async Task<IServiceCallResult> StartUse(long partyId, string usageCode)
    {
        return await callMethod(async () =>
        {
            using SulmServiceClient client = createClient();

            var request = new startUseRequest
            {
                partyId = partyId,
                usageCode = usageCode
            };
            _logger.LogSerializedObject("StartUseAsync", request);
            await client.startUseAsync(getCallerContext(), getCorrelationContext(), request);

            return new SuccessfulServiceCallResult();
        });
    }

    private CallerContext getCallerContext()
        => new CallerContext
        {
            application = "NOBY",
            callerId = "NOBY"
        };

    private CorrelationContext getCorrelationContext()
        => new CorrelationContext
        {
            application = "NOBY",
            id = Activity.Current?.TraceId.ToHexString() ?? ""
        };

    public RealSulmClient(SulmConfiguration configuration, ILogger<RealSulmClient> logger)
        : base(Versions.V1, configuration, logger)
    {
    }

    private SulmServiceClient createClient()
        => new SulmServiceClient(createHttpBinding(), createEndpoint());
}
