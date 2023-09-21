using Microsoft.Extensions.Logging;

namespace ExternalServices.Sulm.V1;

internal sealed class MockSulmClient 
    : ISulmClient
{
    private readonly ILogger<MockSulmClient> _logger;

    public MockSulmClient(ILogger<MockSulmClient> logger)
    {
        _logger = logger;
    }

    public Task StopUse(long kbCustomerId, IList<CIS.Foms.Types.UserIdentity> userIdentities, string purposeCode, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SULM MOCK - STOP USE");
        return Task.CompletedTask;
    }

    public Task StartUse(long kbCustomerId, IList<CIS.Foms.Types.UserIdentity> userIdentities, string purposeCode, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SULM MOCK - START USE");
        return Task.CompletedTask;
    }
}
