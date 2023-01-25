using ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper;
using CIS.Infrastructure.Logging;
using CIS.Core.Exceptions;

namespace ExternalServices.EasSimulationHT.V6;

internal sealed class RealEasSimulationHTClient 
    : Shared.BaseClient<RealEasSimulationHTClient>, IEasSimulationHTClient
{
    public RealEasSimulationHTClient(CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<IEasSimulationHTClient> configuration, ILogger<RealEasSimulationHTClient> logger)
       : base(configuration, logger) { }

    public async Task<SimulationHTResponse> RunSimulationHT(SimulationHTRequest request)
    {
        return await callMethod<SimulationHTResponse>(async () =>
        {
            using HT_WS_SB_ServicesClient client = createClient();
            
            _logger.LogSerializedObject("SimulationHTRequest", request);
            var result = client.SimulationHT(request);
            _logger.LogSerializedObject("SimulationHTResponse", result);

            if ((result.errorInfo?.kodChyby ?? 0) != 0)
            {
                var message = $"Error occured during call external service EAS [{result.errorInfo?.kodChyby} : {result.errorInfo?.textChyby}]";
                _logger.LogWarning(message);
                throw new CisValidationException(10020, result.errorInfo!.textChyby);
            }

            return result;
        });
    }

    public async Task<WFS_FindItem[]> FindTasks(WFS_Header header, WFS_Find_ByCaseId message)
    {
        return await callMethod<WFS_FindItem[]>(async () =>
        {
            using HT_WS_SB_ServicesClient client = createClient();

            _logger.LogSerializedObject("FindTasksRequest", new { header, message });
            var result = client.WFS_FindTasks(header, message);
            _logger.LogSerializedObject("FindTasksResponse", result);

            return result.tasks;
        });
    }

    private HT_WS_SB_ServicesClient createClient()
      => new HT_WS_SB_ServicesClient(createHttpBinding(), createEndpoint());
}