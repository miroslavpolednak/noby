﻿using Microsoft.Extensions.Logging;

#pragma warning disable CA1848 // Use the LoggerMessage delegates

namespace ExternalServices.Sulm.V1;

internal sealed class MockSulmClient 
    : ISulmClient
{
    private readonly ILogger<MockSulmClient> _logger;

    public MockSulmClient(ILogger<MockSulmClient> logger)
    {
        _logger = logger;
    }

    public Task StopUse(long kbCustomerId, string purposeCode, IList<SharedTypes.Types.UserIdentity>? userIdentities, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SULM MOCK - STOP USE");
        return Task.CompletedTask;
    }

    public Task StartUse(long kbCustomerId, string purposeCode, IList<SharedTypes.Types.UserIdentity>? userIdentities, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SULM MOCK - START USE");
        return Task.CompletedTask;
    }
}
