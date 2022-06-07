using ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper;

namespace ExternalServices.EasSimulationHT.V6;

internal sealed class MockEasSimulationHTClient : IEasSimulationHTClient
{
    public Versions Version => throw new NotImplementedException();

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IServiceCallResult> RunSimulationHT(SimulationHTRequest request)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new SuccessfulServiceCallResult<SimulationHTResponse>(new SimulationHTResponse { });
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IServiceCallResult> FindTasks(WFS_Header header, WFS_Find_ByCaseId message)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return new SuccessfulServiceCallResult<SimulationHTResponse>(new SimulationHTResponse { });
    }
}
