using CIS.Core.Results;

namespace FOMS.DocumentProcessing;

internal class BaseDocumentProcessor
{
    protected readonly int _salesArrangementId;
    protected readonly ServiceAccessor _serviceAccessor;

    public BaseDocumentProcessor(ServiceAccessor serviceAccessor, int salesArrangementId)
    {
        _salesArrangementId = salesArrangementId;
        _serviceAccessor = serviceAccessor;
    }

    /*protected async Task<TContract> getSalesArrangementData<TContract>() where TContract : class
    {
        var service = _serviceAccessor.GetRequiredService<DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction>();
        return resolveSalesArrangementData<TContract>(await service.GetSalesArrangementData(_salesArrangementId));
    }

    private TContract resolveSalesArrangementData<TContract>(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementDataResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };*/
}
