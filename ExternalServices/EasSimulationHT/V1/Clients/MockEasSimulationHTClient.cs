using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

namespace ExternalServices.EasSimulationHT.V1;

internal sealed class MockEasSimulationHTClient 
    : IEasSimulationHTClient
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<SimulationHTResponse> RunSimulationHT(SimulationHTRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new SimulationHTResponse { };
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<WFS_FindItem[]> FindTasks(WFS_Header header, WFS_Find_ByCaseId message, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new WFS_FindItem[0];
    }
}
