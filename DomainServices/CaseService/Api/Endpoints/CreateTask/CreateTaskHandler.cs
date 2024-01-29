using SharedTypes.Enums;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.SalesArrangementService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, CreateTaskResponse>
{
    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> metadata = new()
        {
            { getTaskTypeKey(), request.TaskRequest },
            { "ukol_uver_id", request.CaseId.ToString(CultureInfo.InvariantCulture) },
            { "ukol_mandant", "2" }
        };

        MapPriceException(metadata, request.PriceException);

        // subtype
        if (request.TaskTypeId == 3)
        {
            metadata.Add("ukol_konzultace_oblast", $"{request.TaskSubtypeId}");

            if (request.TaskSubtypeId == 1 || request.TaskSubtypeId == 7)
            {
                metadata.Add("ukol_konzultace_order_id", $"{request.OrderId}");
            }
        }

        // ID dokumentu
        if (request.TaskDocumentsId?.Any() ?? false)
        {
            metadata.Add("wfl_refobj_dokumenty", string.Join(",", request.TaskDocumentsId) + ",");
        }

        var result = await _sbWebApi.CreateTask(new ExternalServices.SbWebApi.Dto.CreateTask.CreateTaskRequest
        {
            ProcessId = request.ProcessId is not null ? Convert.ToInt32(request.ProcessId, CultureInfo.InvariantCulture) : default, //IT anal neni schopna rict co s tim
            TaskTypeId = request.TaskTypeId,
            Metadata = metadata
        }, cancellationToken);

        // nastavit flow switche
        await setFlowSwitches(request, cancellationToken);

        return new CreateTaskResponse
        {
            TaskIdSB = result.TaskIdSB,
            TaskId = result.TaskId
        };

        string getTaskTypeKey() => request.TaskTypeId switch
        {
            7 => "ukol_predanihs_pozadavek",
            3 => "ukol_konzultace_pozadavek",
            2 => "ukol_overeni_pozadavek",
            _ => throw new NotImplementedException($"TaskTypeId {request.TaskTypeId} is not supported")
        };
    }

    private async Task setFlowSwitches(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (request.TaskTypeId == 2)
        {
            var salesArrangementId = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken))
                .First()
                .SalesArrangementId;
            await _salesArrangementService.SetFlowSwitch(salesArrangementId, FlowSwitches.DoesWflTaskForIPExist, true, cancellationToken);
        }
    }

    private static void MapPriceException(Dictionary<string, string> metadata, TaskPriceException? priceException)
    {
        if (priceException is null)
            return;

        metadata.Add("ukol_overeni_ic_sazba_dat_do", priceException.Expiration is not null ?
            ((DateOnly)priceException.Expiration!).ToSbFormat() 
            : string.Empty);
        metadata.Add("ukol_overeni_ic_sazba_nabid", priceException.LoanInterestRate.LoanInterestRate.ToSbFormat());
        metadata.Add("ukol_overeni_ic_sazba_vysled", priceException.LoanInterestRate.LoanInterestRateProvided.ToSbFormat());
        metadata.Add("ukol_overeni_ic_sazba_typ", priceException.LoanInterestRate.LoanInterestRateAnnouncedType.HasValue ?
            priceException.LoanInterestRate.LoanInterestRateAnnouncedType.Value.ToString(CultureInfo.InvariantCulture)
            : string.Empty);
        metadata.Add("ukol_overeni_ic_sazba_sleva", priceException.LoanInterestRate.LoanInterestRateDiscount.ToSbFormat());
        metadata.Add("ukol_overeni_ic_kod_produktu", priceException.ProductTypeId.HasValue ?
            priceException.ProductTypeId.Value.ToString(CultureInfo.InvariantCulture)
            : string.Empty);
        metadata.Add("ukol_overeni_ic_vyse_uveru", priceException.LoanAmount.HasValue ?
            priceException.LoanAmount.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
        metadata.Add("ukol_overeni_ic_splatnost_uveru_poc_mes", priceException.LoanDuration.HasValue ?
            priceException.LoanDuration.Value.ToString(CultureInfo.InvariantCulture)
            : string.Empty);
        metadata.Add("ukol_overeni_ic_uver_ltv", priceException.LoanToValue.HasValue ?
            priceException.LoanToValue.Value.ToString(CultureInfo.InvariantCulture)
            : string.Empty);
        metadata.Add("ukol_overeni_ic_fixace_uveru_poc_mes", priceException.FixedRatePeriod.HasValue ?
            priceException.FixedRatePeriod.Value.ToString(CultureInfo.InvariantCulture)
            : string.Empty);

        for (var i = 0; i < priceException.Fees.Count; i++)
        {
            metadata.Add($"ukol_overeni_ic_popl_kodsb{i + 1}", priceException.Fees![i].FeeId.ToString(CultureInfo.InvariantCulture));
            metadata.Add($"ukol_overeni_ic_popl_sazeb{i + 1}", priceException.Fees![i].TariffSum.ToSbFormat());
            metadata.Add($"ukol_overeni_ic_popl_vysl{i + 1}", priceException.Fees![i].FinalSum.ToSbFormat());
            metadata.Add($"ukol_overeni_ic_popl_sleva_perc{i + 1}", priceException.Fees![i].DiscountPercentage.ToSbFormat());
        }

        foreach (var item in priceException.AppliedMarketingActionsCodes
                                           .Select(GetMarketingActionValue)
                                           .Where(value => value.HasValue)
                                           .Select((value, index) => new { value = (int)value!, index }))
        {
            metadata.Add($"ukol_overeni_ic_skladacka_ma{item.index + 1}", item.value.ToString(CultureInfo.InvariantCulture));
        }

        static int? GetMarketingActionValue(string key) => key switch
        {
            "DOMICILACE" => 1,
            "RZP" => 2,
            "VYSE_PRIJMU_UVERU" => 3,
            "POJIST_NEM" => 4,
            _ => null
        };
    }

    private readonly ISbWebApiClient _sbWebApi;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public CreateTaskHandler(ISbWebApiClient sbWebApi, ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _sbWebApi = sbWebApi;
    }
}
