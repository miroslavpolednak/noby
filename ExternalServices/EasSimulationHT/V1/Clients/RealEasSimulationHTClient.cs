using CIS.Core.Exceptions;
using CIS.Core.Extensions;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.ExternalServicesHelpers.Soap;
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

    public async Task<decimal> RunSimulationRefixation(long caseId, decimal newInterestRate, DateTime interestRateValidFrom, int fixedRatePeriod, DateTime futureInterestRateValidTo, CancellationToken cancellationToken)
    {
        var result = await callMethod(async () =>
        {
            var request = new SimHu_RetenceHedge_Request
            {
                settings = new()
                {
                    uverId = Convert.ToInt32(caseId),
                    mode = 2,
                    novaSazba = newInterestRate,
                    novaSazbaOd = interestRateValidFrom,
                    periodaFixace = fixedRatePeriod,
                    novaSplatnost = futureInterestRateValidTo
                }
            };

            return await Client.SimHu_RetenceHedgeAsync(request).WithCancellation(cancellationToken);
        }, r => r.errorInfo, 10028);

        return result.vysledky?.novaVyseSplatky ?? 0;
    }

    public async Task<decimal> RunSimulationRetention(long caseId, decimal newInterestRate, DateTime interestRateValidFrom, CancellationToken cancellationToken)
    {
        var result = await callMethod(async () =>
        {
            var request = new SimHu_RetenceHedge_Request
            {
                settings = new()
                {
                    uverId = Convert.ToInt32(caseId),
                    mode = 1,
                    novaSazba = newInterestRate,
                    novaSazbaOd = interestRateValidFrom
                }
            };

            return await Client.SimHu_RetenceHedgeAsync(request).WithCancellation(cancellationToken);
        }, r => r.errorInfo, 10028);

        return result.vysledky?.novaVyseSplatky ?? 0;
    }

    public async Task<WFS_FindItem[]> FindTasks(WFS_Header header, WFS_Find_ByCaseId message, CancellationToken cancellationToken)
    {
        return await callMethod<WFS_FindItem[]>(async () => (await Client.WFS_FindTasksAsync(header, message).WithCancellation(cancellationToken)).tasks);
    }

    public async Task<SimulationHTResponse> RunSimulationHT(SimulationHTRequest request, CancellationToken cancellationToken)
    {
        return await callMethod(async () => await Client.SimulationHTAsync(request).WithCancellation(cancellationToken), r => r.errorInfo, 10020);
    }

    private async Task<TResult> callMethod<TResult>(Func<Task<TResult>> fce, Func<TResult, SimErrorInfo?> errorInfoGetter, int exceptionCode)
    {
        var result = await base.callMethod(fce);
        var errorInfo = errorInfoGetter(result);

        if ((errorInfo?.kodChyby ?? 0) != 0)
        {
            Logger.ExternalServiceResponseError($"Error occured during call external service EAS [{errorInfo?.kodChyby} : {errorInfo?.textChyby}]");
            throw new CisValidationException(exceptionCode, errorInfo!.textChyby);
        }

        return result;
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
