﻿using ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper;
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

            if (result.errorInfo != null)
            {
                var message = $"Error occured during call external service EAS [{result.errorInfo.kodChyby} : {result.errorInfo.textChyby}]";
                _logger.LogWarning(message);
                return new ErrorServiceCallResult(99999, message); //TODO: error code
            }

            return new SuccessfulServiceCallResult<SimulationHTResponse>(result);
        });
    }


    private HT_WS_SB_ServicesClient createClient()
      => new HT_WS_SB_ServicesClient(createHttpBinding(), createEndpoint());
}