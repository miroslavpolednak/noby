﻿using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

namespace ExternalServices.EasSimulationHT.V1;

internal sealed class MockEasSimulationHTClient 
    : IEasSimulationHTClient
{
    public Task<SimulationHTResponse> RunSimulationHT(SimulationHTRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SimulationHTResponse());
    }

    public Task<WFS_FindItem[]> FindTasks(WFS_Header header, WFS_Find_ByCaseId message, CancellationToken cancellationToken)
    {
        return Task.FromResult(Array.Empty<WFS_FindItem>());
    }

    public Task<decimal> RunSimulationRetention(long caseId, decimal newInterestRate, DateTime interestRateValidFrom, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> RunSimulationRefixation(long caseId, decimal newInterestRate, DateTime interestRateValidFrom, int fixedRatePeriod, DateTime futureInterestRateValidTo, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
