using ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper;

namespace ExternalServices.EasSimulationHT.V6;

public interface IEasSimulationHTClient
{
    Versions Version { get; }

    /// <summary>
    /// Call simulation HT method
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[SimulationHTResponse]
    /// </returns>
    Task<IServiceCallResult> RunSimulationHT(SimulationHTRequest request);

    /// <summary>
    /// Call method to find tasks by CaseId
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[WFS_FindItem[]]
    /// </returns>
    Task<IServiceCallResult> FindTasks(WFS_Header header, WFS_Find_ByCaseId message);

}