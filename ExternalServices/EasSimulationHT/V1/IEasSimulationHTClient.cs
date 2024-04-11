using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

namespace ExternalServices.EasSimulationHT.V1;

public interface IEasSimulationHTClient
    : IExternalServiceClient
{
    Task<Dto.RefinancingSimulationResult> RunSimulationRefixation(long caseId, decimal interestRate, DateTime interestRateValidFrom, int fixedRatePeriod, CancellationToken cancellationToken);

    Task<Dto.RefinancingSimulationResult> RunSimulationRetention(long caseId, decimal interestRate, DateTime interestRateValidFrom, CancellationToken cancellationToken);

    /// <summary>
    /// Call simulation HT method
    /// </summary>
    Task<SimulationHTResponse> RunSimulationHT(SimulationHTRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Call method to find tasks by CaseId
    /// </summary>
    Task<WFS_FindItem[]> FindTasks(WFS_Header header, WFS_Find_ByCaseId message, CancellationToken cancellationToken);

    const string Version = "V1";
}