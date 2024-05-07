using ExternalServices.EasSimulationHT.Dto;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

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

    public Task<Dto.RefinancingSimulationResult> RunSimulationRefixation(long caseId, decimal interestRate, DateTime interestRateValidFrom, int fixedRatePeriod, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Dto.RefinancingSimulationResult> RunSimulationRetention(long caseId, decimal interestRate, DateTime interestRateValidFrom, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<MortgageExtraPaymentResult> RunSimulationExtraPayment(long caseId, DateTime extraPaymentDate, decimal? extraPaymentAmount, int extraPaymentReasonId, bool isExtraPaymentFullyRepaid, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
