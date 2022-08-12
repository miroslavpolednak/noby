using ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper;
using CIS.Infrastructure.Logging;


namespace ExternalServices.EasSimulationHT.V6;

internal sealed class RealEasSimulationHTClient : Shared.BaseClient<RealEasSimulationHTClient>, IEasSimulationHTClient
{
    public RealEasSimulationHTClient(EasSimulationHTConfiguration configuration, ILogger<RealEasSimulationHTClient> logger)
       : base(Versions.V6, configuration, logger) { }


    public async Task<IServiceCallResult> RunSimulationHT(SimulationHTRequest request)
    {
        return await callMethod(async () =>
        {
            using HT_WS_SB_ServicesClient client = createClient();

            _logger.LogSerializedObject("SimulationHTRequest", request);
            var result = await client.SimulationHTAsync(request);
            _logger.LogSerializedObject("SimulationHTResponse", result);

            //if (result.errorInfo != null)
            //{
            //    var message = $"Error occured during call external service EAS [{result.errorInfo.kodChyby} : {result.errorInfo.textChyby}]";
            //    _logger.LogWarning(message);
            //    return new ErrorServiceCallResult(99999, message); //TODO: error code
            //}

            if ((result.errorInfo?.kodChyby ?? 0) != 0)
            {
                var message = $"Error occured during call external service EAS [{result.errorInfo?.kodChyby} : {result.errorInfo?.textChyby}]";
                _logger.LogWarning(message);
                return new ErrorServiceCallResult(9202, message);
            }

            return new SuccessfulServiceCallResult<SimulationHTResponse>(result);
        });
    }

    public async Task<IServiceCallResult> FindTasks(WFS_Header header, WFS_Find_ByCaseId message)
    {
        return await callMethod(async () =>
        {
            using HT_WS_SB_ServicesClient client = createClient();

            _logger.LogSerializedObject("FindTasksRequest", new { header, message });
            var result = await client.WFS_FindTasksAsync(header, message);
            _logger.LogSerializedObject("FindTasksResponse", result);

            return new SuccessfulServiceCallResult<WFS_FindItem[]>(result.tasks);
        });
    }

    private HT_WS_SB_ServicesClient createClient()
      => new HT_WS_SB_ServicesClient(createHttpBinding(), createEndpoint());
}