using CIS.Core.Results;

namespace FOMS.DocumentProcessing;

internal abstract class BaseDocumentProcessor
{
    // instance aktualniho SA
    protected readonly DomainServices.SalesArrangementService.Contracts.SalesArrangement _salesArrangement;
    // pristup do DI
    protected readonly ServiceAccessor _serviceAccessor;

    // instance SA service
    protected DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService
    {
        get => _salesArrangementServiceField is null ? _serviceAccessor.GetRequiredService<DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction>() : _salesArrangementServiceField;
    }
    private DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction? _salesArrangementServiceField = null;

    // vraci informace o aktualne prihlasenem uzivateli
    protected async Task<DocumentContracts.SharedModels.DealerInfo> getCurrentUserInfo()
    {
        if (_currentUser is not null) return _currentUser;

        var userAccessor = _serviceAccessor.GetRequiredService<CIS.Core.Security.ICurrentUserAccessor>();
        var svc = _serviceAccessor.GetRequiredService<DomainServices.UserService.Abstraction.IUserServiceAbstraction>();

        var userInstance = ServiceCallResult.Resolve<DomainServices.UserService.Contracts.User>(await svc.GetUserByLogin(userAccessor.User.Login));
        _currentUser = new()
        {
            CPM = userInstance.CPM,
            ICP = userInstance.ICP,
            FullName = userInstance.FullName
        };

        return _currentUser;
    }
    private DocumentContracts.SharedModels.DealerInfo? _currentUser = null;

    protected async Task<TContract?> getSalesArrangementData<TContract>() where TContract : class
    {
        /*var result = ServiceCallResult.Resolve<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementDataResponse>(await _salesArrangementService.GetSalesArrangementData(_salesArrangement.SalesArrangementId));
        if (result.SalesArrangementDataId.HasValue && !string.IsNullOrEmpty(result.Data))
            return System.Text.Json.JsonSerializer.Deserialize<TContract>(result.Data) ?? throw new Exception($"Deserialization of contract {typeof(TContract)} failed");
        else*/
            return null;
    }

    protected async void saveSalesArrangementData(object data)
    {
        string convertedSaObject = System.Text.Json.JsonSerializer.Serialize(data);
        //ServiceCallResult.Resolve(await _salesArrangementService.UpdateSalesArrangementData(_salesArrangement.SalesArrangementId, convertedSaObject));
    }

    public BaseDocumentProcessor(ServiceAccessor serviceAccessor, DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement)
    {
        _salesArrangement = salesArrangement;
        _serviceAccessor = serviceAccessor;
    }
}
