﻿using CIS.Core.Exceptions;
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

    public async Task<Dto.MortgageExtraPaymentResult> RunSimulationExtraPayment(long caseId, DateTime extraPaymentDate, decimal? extraPaymentAmount, int extraPaymentReasonId, bool isExtraPaymentFullyRepaid, CancellationToken cancellationToken)
    {
        var result = await callMethod(async () =>
        {
            var request = new SimHT_UVN_Request
            {
                settings = new()
                {
                    uverId = Convert.ToInt32(caseId),
                    mode = 2,
                    typMimoradneSplatky = isExtraPaymentFullyRepaid,
                    duvodMimoradneSplatky = extraPaymentReasonId,
                    sumaMimoradneSplatky = extraPaymentAmount ?? 0,
                    datumMimoradneSplatky = extraPaymentDate
                }
            };

            return await Client.SimHT_UVNAsync(request).WithCancellation(cancellationToken);
        }, r => r.errorInfo, 10028);

        return new Dto.MortgageExtraPaymentResult
        {
            ExtraPaymentAmount = result.dataProDopis.extra_payment_sum,
            FeeAmount = result.dataProDopis.uvn_amount,
            InterestAmount = result.dataProDopis.interest_amount,
            InterestCovid = result.dataProDopis.interest_covid,
            InterestOnLate = result.dataProDopis.interest_on_late,
            IsExtraPaymentComplete = result.dataProDopis.full_repayment,
            IsLoanOverdue = result.dataProDopis.loan_overdue,
			IsInstallmentReduced = result.dataProDopis.payment_reduction,
            NewMaturityDate = result.dataProDopis.new_maturity_date,
            NewPaymentAmount = result.dataProDopis.new_payment_amount,
            OtherUnpaidFees = result.dataProDopis.other_unpaid_fees,
            PrincipalAmount = result.dataProDopis.principal_amount,
            FeeTypeId = result.vysledky.typPokuty,
            FeeCalculationBase = result.vysledky.zakladProVypocetPokuty,
            FeeAmountContracted = result.vysledky.smluvniPokuty,
            FixedRateSanctionFreePeriodFrom = result.vysledky.obdobiFixaceOd,
            FixedRateSanctionFreePeriodTo = result.vysledky.obdobiFixaceDo,
            AnnualSanctionFreePeriodFrom = result.vysledky.obdobiVyrociOd,
            AnnualSanctionFreePeriodTo = result.vysledky.obdobiVyrociDo,
            SanctionFreeAmount = result.vysledky.castkaBezPoplatkuVeVyroci
        };
    }

    public async Task<Dto.RefinancingSimulationResult> RunSimulationRefixation(long caseId, decimal interestRate, DateTime interestRateValidFrom, int fixedRatePeriod, CancellationToken cancellationToken)
    {
        var result = await callMethod(async () =>
        {
            var request = new SimHu_RetenceHedge_Request
            {
                settings = new()
                {
                    uverId = Convert.ToInt32(caseId),
                    mode = 2,
                    novaSazba = interestRate,
                    novaSazbaOd = interestRateValidFrom,
                    periodaFixace = fixedRatePeriod
                }
            };

            return await Client.SimHu_RetenceHedgeAsync(request).WithCancellation(cancellationToken);
        }, r => r.errorInfo, 10028);

        return new Dto.RefinancingSimulationResult
        {
            LoanPaymentAmount = result.vysledky?.novaVyseSplatky ?? 0,
            LoanPaymentsCount = result.vysledky?.pocetSplatek ?? 0,
            MaturityDate = result.vysledky?.novaSplatnost ?? DateTime.MinValue
        };
    }

    public async Task<Dto.RefinancingSimulationResult> RunSimulationRetention(long caseId, decimal interestRate, DateTime interestRateValidFrom, CancellationToken cancellationToken)
    {
        var result = await callMethod(async () =>
        {
            var request = new SimHu_RetenceHedge_Request
            {
                settings = new()
                {
                    uverId = Convert.ToInt32(caseId),
                    mode = 1,
                    novaSazba = interestRate,
                    novaSazbaOd = interestRateValidFrom
                }
            };

            return await Client.SimHu_RetenceHedgeAsync(request).WithCancellation(cancellationToken);
        }, r => r.errorInfo, 10028);

        return new Dto.RefinancingSimulationResult
        {
            LoanPaymentAmount = result.vysledky?.novaVyseSplatky ?? 0,
            LoanPaymentsCount = result.vysledky?.pocetSplatek ?? 0,
            MaturityDate = result.vysledky?.novaSplatnost ?? DateTime.MinValue
        };
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
