using CIS.Core.Exceptions;
using CIS.Core.Extensions;
using CIS.Infrastructure.ExternalServicesHelpers.BaseClasses;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.Logging;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;
using System.ServiceModel.Channels;

namespace ExternalServices.EasSimulationHT.V1;
public class RealEasSimulationHTClient : SoapClientBase<HT_WS_SB_ServicesClient, IHT_WS_SB_Services>, IEasSimulationHTClient
{
    public RealEasSimulationHTClient(
        ILogger<RealEasSimulationHTClient> logger,
        IExternalServiceConfiguration<IEasSimulationHTClient> configuration)
        : base(configuration, logger)
    {
    }

    protected override string ServiceName => StartupExtensions.ServiceName;

    public async Task<WFS_FindItem[]> FindTasks(WFS_Header header, WFS_Find_ByCaseId message, CancellationToken cancellationToken)
    {
        return await callMethod<WFS_FindItem[]>(async () =>
        {
            var result = await Client.WFS_FindTasksAsync(header, message).WithCancellation(cancellationToken);
            return result.tasks;
        });
    }

    public async Task<SimulationHTResponse> RunSimulationHT(SimulationHTRequest request, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {
            var result = await Client.SimulationHTAsync(request).WithCancellation(cancellationToken);

            if ((result.errorInfo?.kodChyby ?? 0) != 0)
            {
                Logger.ExtServiceResponseError($"Error occured during call external service EAS [{result.errorInfo?.kodChyby} : {result.errorInfo?.textChyby}]");
                throw new CisValidationException(10020, result.errorInfo!.textChyby);
            }
            return result;
        });
    }

    protected override Binding CreateBinding()
    {
        var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

        if (Configuration.RequestTimeout.HasValue)
        {
            basicHttpBinding.SendTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout.Value);
            basicHttpBinding.CloseTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout.Value);
        }
        basicHttpBinding.MaxReceivedMessageSize = 1500000;
        basicHttpBinding.ReaderQuotas.MaxArrayLength = 1500000;

        return basicHttpBinding;
    }
}
