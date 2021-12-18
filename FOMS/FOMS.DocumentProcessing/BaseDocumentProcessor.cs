using CIS.Core.Results;

namespace FOMS.DocumentProcessing;

internal class BaseDocumentProcessor
{
    // instance aktualniho SA
    protected readonly DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse _salesArrangement;
    // pristup do DI
    protected readonly ServiceAccessor _serviceAccessor;
    // instance SA service
    protected DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService
    {
        get => _salesArrangementServiceField is null ? _serviceAccessor.GetRequiredService<DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction>() : _salesArrangementServiceField;
    }
    private DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction? _salesArrangementServiceField = null;

    public BaseDocumentProcessor(ServiceAccessor serviceAccessor, DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse salesArrangement)
    {
        _salesArrangement = salesArrangement;
        _serviceAccessor = serviceAccessor;
    }

    protected async Task<TContract?> getSalesArrangementData<TContract>() where TContract : class
    {
        var result = resolveGetSalesArrangementData(await _salesArrangementService.GetSalesArrangementData(_salesArrangement.SalesArrangementId));
        if (result.SalesArrangementDataId.HasValue && !string.IsNullOrEmpty(result.Data))
            return System.Text.Json.JsonSerializer.Deserialize<TContract>(result.Data) ?? throw new Exception($"Deserialization of contract {typeof(TContract)} failed");
        else
            return null;
    }

    protected async void saveSalesArrangementData(object data)
    {
        string convertedSaObject = System.Text.Json.JsonSerializer.Serialize(data);
        resolveSaveSalesArrangementData(await _salesArrangementService.UpdateSalesArrangementData(_salesArrangement.SalesArrangementId, convertedSaObject));
    }

    private DomainServices.SalesArrangementService.Contracts.GetSalesArrangementDataResponse resolveGetSalesArrangementData(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementDataResponse> r => r.Model,
            _ => throw new NotImplementedException("Can't read SA data")
        };

    private bool resolveSaveSalesArrangementData(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult => true,
            _ => throw new NotImplementedException("Can't read SA data")
        };
}
