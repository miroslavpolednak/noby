using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper;

namespace ExternalServices.EasSimulationHT.V6;

public interface IEasSimulationHTClient
    : IExternalServiceClient
{
    /// <summary>
    /// Call simulation HT method
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[SimulationHTResponse]
    /// </returns>
    Task<SimulationHTResponse> RunSimulationHT(SimulationHTRequest request);

    /// <summary>
    /// Call method to find tasks by CaseId
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[WFS_FindItem[]]
    /// </returns>
    Task<WFS_FindItem[]> FindTasks(WFS_Header header, WFS_Find_ByCaseId message);

    const string Version = "V6";
}